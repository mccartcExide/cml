using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CML.Controllers
{
    public class FilesController : Controller
    {
        // GET: Files
        public ActionResult Index()
        {
            //IList<UploadInitialFile> initialFiles =
            //   SessionUploadInitialFilesRepository.GetAllInitialFiles();

            //return View( initialFiles );
            return View();
        }

        public ActionResult Save_Files( IEnumerable<HttpPostedFileBase> files, int id, string parentType)
        {
            if ( files != null )
            {
                foreach ( var file in files )
                {
                    var rep = "Requests";
                    if ( !parentType.Equals( "R" ) )
                        rep = "Tests";
                    string dir =  @"~/Upload/" + rep + "/" + id;

                    if ( !Directory.Exists( Server.MapPath( dir ) ) )
                        Directory.CreateDirectory( Server.MapPath( dir ) );

                    var fileName = Path.GetFileName( file.FileName );
                   

                    var physicalPath = Path.Combine( Server.MapPath( dir ), fileName );
                    var fileExtension = Path.GetExtension( file.FileName );

                    
                     file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content( "" );
        }

        public ActionResult Remove_Files( String[] fileNames, int id, string parentType )
        {
            foreach(var f in fileNames )
            {
                var rep = "Requests";
                if ( !parentType.Equals( "R" ) )
                    rep = "Tests";
                string dir = Server.MapPath( @"~/Upload/" + rep + "/" + id + "/" + f );

                System.IO.File.Delete( dir );

            }

            return Content( "" );
        }

        public FileResult Download( int id, string parentType, string name )
        {
            var rep = "Requests";
            if ( !parentType.Equals( "R" ) )
                rep = "Tests";
            string dir = @"~/Upload/"+rep+"/" + id + "/" + name;
            return File( dir, "application/force-download", Path.GetFileName( dir ) );

            // return View();
        }

        public ActionResult Remove_File( int id, string parentType, string name )
        {
            var rep = "Requests";
            if ( !parentType.Equals( "R" ) )
                rep = "Tests";
            string dir = Server.MapPath(@"~/Upload/" + rep + "/" + id + "/" + name);
            
            System.IO.File.Delete( dir );

            //return Content( "" );
            if ( parentType.Equals( "R" ) )
            {
                return RedirectToAction( "Edit", "Request", new { id = id } );
            }
            else
            {
                return RedirectToAction( "GetTestView", "Test", new { id = id } );
            }
            //
        }

    }
}