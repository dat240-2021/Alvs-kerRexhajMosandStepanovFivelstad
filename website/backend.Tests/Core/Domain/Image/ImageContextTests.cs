using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using backend.Controllers.NewGame.Dto;
using backend.Core.Domain.Images;
using backend.Core.Domain.Images.Pipelines;
using backend.Tests.Helpers;
using Domain.Image;
using Domain.Image.Pipelines;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace backend.Tests.Core.Domain.Image
{
    public class ImageContextTests : DbTest
    {
        public ImageContextTests(ITestOutputHelper output) : base(output)
        {
        }

        //Test AddUserImage pipeline by adding two images. 
        [Fact]
        public void AddNewImage_AddsTwoImages()
        {

            var UserId = Guid.NewGuid();
            List<ImageFile> TestImageList = new List<ImageFile>();

            int index = 1;

            foreach (string file in Directory.GetFiles(Path.Join("..", "..", "..", "Infrastructure", "Data", "Images")))
            {
                var tempImageFile = new ImageFile();
                var image = File.ReadAllBytes(file);
                var b64 = Convert.ToBase64String(image);
                tempImageFile.Id = index;
                tempImageFile.File = b64;
                tempImageFile.Category = 1;
                tempImageFile.Label = $"Test Label {index}";
                tempImageFile.Name = $"Test Name {index}";
                TestImageList.Add(tempImageFile);
                index++;

            }

            var request = new AddUserImage.Request(TestImageList.ToArray(), UserId);

            using (var context = new GameContext(ContextOptions, null))
            {
                context.Database.Migrate();

                context.ImageCategories.Add(new ImageCategory("Test Category 1"));
                context.ImageCategories.Add(new ImageCategory("Test Category 2"));

                var handler = new AddUserImage.Handler(context);

                var result = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

                context.ImageCategories.Count().ShouldBe(2);
                var categoryId = context.ImageCategories.Select(i => i.Id).FirstOrDefault();
                Assert.Equal(1, categoryId);
                var actImageSliceList = context.Images.Select(i => i.Slices);
                Assert.Equal(result.Success.ToString(), true.ToString());
                actImageSliceList.First().Count().ShouldBe(49);
                var actLabel = context.Images.Select(i => i.Label.Label);
                Assert.Equal("Test Label 1", actLabel.First());
            }

        }

        //Test GetCategoryList pipeline by adding 3 categories and retrieving list. 
        [Fact]
        public void AddCategories_And_Get_CategoryList()
        {
            string cat1 = "Famous People";
            string cat2 = "Movies";
            string cat3 = "Landscapes";

            var request = new GetCategoryList.Request();

            using (var context = new GameContext(ContextOptions, null))
            {
                context.Database.Migrate();

                context.ImageCategories.Add(new ImageCategory(cat1));
                context.ImageCategories.Add(new ImageCategory(cat2));
                context.ImageCategories.Add(new ImageCategory(cat3));
                context.SaveChanges();
                var handler = new GetCategoryList.Handler(context);

                var result = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
                Assert.True(result[0].Name.Equals(cat1));
                Assert.True(result[1].Name.Equals(cat2));
                Assert.True(result[2].Name.Equals(cat3));
                Assert.False(result[2].Name.Equals(cat1));
            }
        }
        
        //Test GetImageIdsByCategoryDefaultImages pipeline by adding 2 Images and retrieving list. 
        [Fact]
        public void GetImageIdsByCategory_DefaultList()
        {

            List<int> CategoryIds = new List<int>();
            CategoryIds.Add(1);

            var request = new GetImageIdsByCategoryDefaultImages.Request(CategoryIds);

            using (var context = new GameContext(ContextOptions, null))
            {
                context.Database.Migrate();
                
                var Category1 = new backend.Core.Domain.Images.ImageCategory(1, "Test Category 1");
                var Category2 = new backend.Core.Domain.Images.ImageCategory(2, "Test Category 2");
                //context.ImageCategories.Add(new ImageCategory("Test Category 1"));
                //context.ImageCategories.Add(new ImageCategory("Test Category 2"));
                var Label1 = new backend.Core.Domain.Images.ImageLabel("Test Label 1",Category1);
                var Label2 = new backend.Core.Domain.Images.ImageLabel("Test Label 2",Category1);
                var Label3 = new backend.Core.Domain.Images.ImageLabel("Test Label 3",Category2);
                var Image1 = new backend.Core.Domain.Images.Image();
                var Image2 = new backend.Core.Domain.Images.Image();
                var Image3 = new backend.Core.Domain.Images.Image();
                Image1.SetLabel(Label1);
                Image2.SetLabel(Label2);
                Image3.SetLabel(Label3);
                context.Images.Add(Image1);
                context.Images.Add(Image2);
                context.Images.Add(Image3);
                context.SaveChanges();
 

                var handler = new GetImageIdsByCategoryDefaultImages.Handler(context);

                var result = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

                context.ImageCategories.Count().ShouldBe(2);
                Assert.Equal(1,result.First());
                Assert.Equal(2,result.Last());
            }
        }
        
        //Test GetImageIdsByCategoryDefaultImages pipeline by adding 2 Images and retrieving list. 
        [Fact]
        public void GetImageIdsByCategory_UserList()
        {

            var UserId = new Guid();
            List<int> CategoryIds = new List<int>();
            CategoryIds.Add(1);
            
            var request = new GetImageIdsByCategoryUserImages.Request(CategoryIds,UserId);

            using (var context = new GameContext(ContextOptions, null))
            {
                context.Database.Migrate();
                
                var Category1 = new backend.Core.Domain.Images.ImageCategory(1, "Test Category 1");
                var Category2 = new backend.Core.Domain.Images.ImageCategory(2, "Test Category 2");
                //context.ImageCategories.Add(new ImageCategory("Test Category 1"));
                //context.ImageCategories.Add(new ImageCategory("Test Category 2"));
                var Label1 = new backend.Core.Domain.Images.ImageLabel("Test Label 1",Category1);
                var Label2 = new backend.Core.Domain.Images.ImageLabel("Test Label 2",Category1);
                var Label3 = new backend.Core.Domain.Images.ImageLabel("Test Label 3",Category2);
                var Image1 = new backend.Core.Domain.Images.Image(UserId,Label1);
                var Image2 = new backend.Core.Domain.Images.Image(UserId,Label2);
                var Image3 = new backend.Core.Domain.Images.Image(UserId,Label3);
                context.Images.Add(Image1);
                context.Images.Add(Image2);
                context.Images.Add(Image3);
                context.SaveChanges();
 

                var handler = new GetImageIdsByCategoryUserImages.Handler(context);

                var result = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

                context.ImageCategories.Count().ShouldBe(2);
                Assert.Equal(1,result.First());
                Assert.Equal(2,result.Last());
            }
        }
    }
}

