using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using backend.Core.Domain.Image;
using backend.Core.Domain.Image.Pipelines;
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

        //Test AddUserImage pipeline by adding one image. 
        [Fact]
        public void AddNewImage_AddsOneImage()
        {
            var UserId = Guid.NewGuid();
            var ImageLabel = "Test Label";
            var Category = "Test Category";
            var ImageList = new List<(byte[], int)>();


            foreach (string file in Directory.GetFiles(("../../../Infrastructure/Data/Images")) )
            {
                int number;
                if (int.TryParse(file.Split("/").Last().Replace(".png", ""), out number)){

                        ImageList.Add((File.ReadAllBytes(file), number ));

                }
            }

            var request = new AddUserImage.Request(ImageList,UserId,ImageLabel,Category);

            using (var context = new GameContext(ContextOptions, null))
            {
                context.Database.Migrate();

                var handler = new AddUserImage.Handler(context);

                _ = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
            }
            using (var context = new GameContext(ContextOptions, null))
            {
                context.Images.Count().ShouldBe(1);
                context.ImageCategories.Count().ShouldBe(1);
                var actCategory = context.Images.Select(i => i.Category);
                var actLabel = context.Images.Select(i => i.Label.Label);
                var actImageList = context.Images.Select(i => i.Slices);
                Assert.Equal(Category,actCategory.First());
                Assert.Equal(ImageLabel,actLabel.First());
                Assert.Equal(5, actImageList.First().Count());
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
                Assert.True(result[0].Equals(cat1));
                Assert.True(result[1].Equals(cat2));
                Assert.True(result[2].Equals(cat3));
                Assert.False(result[2].Equals(cat1));
            }
        }
    }
}
