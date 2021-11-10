using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame;
using backend.Core.Domain.BackendGame.Models;
using backend.Core.Domain.BackendGame.Pipelines;
using Domain.Authentication;
using Domain.Image;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedKernel;


namespace Infrastructure.Data
{
    public class GameContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        private readonly IMediator _mediator;

        public GameContext(DbContextOptions configuration, IMediator mediator) : base(configuration)
        {
            _mediator = mediator;
        }

        public DbSet<Image> Images { get; set; } = null!;
        public DbSet<ImageCategory> ImageCategories { get; set; } = null!;
		public DbSet<Score> Scores { get; set; } = null!;
		public DbSet<Game> Games { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
	}

        internal class  ImagePreprocessor{

            // Relative path from current dir.
            public List<Image> Images = new List<Image>();

            public List<ImageCategory> Categories = new List<ImageCategory>();
            public List<ImageLabel> Labels = new List<ImageLabel>();
            public List<(string, int)> LabelMapping = new List<(string,int)>();
            private List<String> Path = new List<string>{Directory.GetCurrentDirectory(),"Infrastructure","ImageFetch","DownloadedFiles","Images"};

            public void Parse(){

                CheckFiles();

                //check each folder use the folder name to create an image, add it to images list
                foreach ( string dirFile in Directory.GetDirectories(System.IO.Path.Combine(Path.ToArray())) ){
                    var newImage = new Image(dirFile.Split("/").Last().Replace("_scattered", ""));

                    foreach ( string filePath in Directory.GetFiles(dirFile) ){
                            int number;
                            if (int.TryParse(filePath.Split("/").Last().Replace(".png", ""), out number)){

                            newImage.AddImageSlice(File.ReadAllBytes(filePath), number );

                            }
                    }
                    Images.Add(newImage);
                };

                foreach (string line in System.IO.File.ReadLines( ModifyPath("Categories.csv",2) ))
                {
                    var categoryLine = line.Split(":");
                    Categories.Add(new ImageCategory(Int32.Parse(categoryLine[0]),categoryLine[1]));
                }

                //check each line in the label mapping file create a category and add it to the categories list
                foreach (string line in System.IO.File.ReadLines( ModifyPath("label_mapping_with_categories.csv",2) ))
                {
                    var labelLine = line.Split(":");
                    Labels.Add(
                        new ImageLabel(Int32.Parse(labelLine[0]),labelLine[1], Categories.Find(x => x.Id==Int32.Parse(labelLine[2]))));
                }


                //check each line in the image mapping file, add it to the mapping list
                foreach (string line in System.IO.File.ReadLines( ModifyPath("image_mapping.csv",1) ))
                {
                    var mappingLine = line.Split(" ");
                    string image = mappingLine[0];
                    int labelid = Int32.Parse(mappingLine[1]);
                    var img = Images.Find(x=> x.ImportId==image);

                    if (img!=null){
                        img.SetLabel(Labels.Find( x=> x.Id == labelid));
                    }
                }


            }

        //Checks if all folders exist if so it uses them, otherwise starts a download using shellscript.
        public void CheckFiles(){
                var folders_ok = false;
                try{
                    folders_ok = Directory.GetDirectories(System.IO.Path.Combine(Path.ToArray())).Count() <= 299;
                } catch {}

                if ( !folders_ok ){
                    Console.WriteLine("Downloading Files");
                    var script = new ProcessStartInfo( ModifyPath("DownloadScript.sh",2),ModifyPath("",2)) ;
                    var process = Process.Start(script);
                    process.WaitForExit();

                    //if script fails
                    if (process.ExitCode != 0){
                        throw new FileLoadException("Something went wrong while downloading files!");
                    }
                } else {
                    Console.WriteLine("Using Downloaded Files!");
                }
        }

        public string ModifyPath(string add,int remove){
                var tmp = Path.GetRange(0,Path.Count-remove);
                tmp.Add(add);
                return System.IO.Path.Combine(tmp.ToArray());
        }
    }
}