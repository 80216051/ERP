using System;
using System.Web;

namespace Utility.URLRewriter
{
    /// <summary>
    /// The base class for module rewriting.  This class is abstract, and therefore must be derived from.
    /// </summary>
    /// <remarks>Provides the essential base functionality for a rewriter using the HttpModule approach.</remarks>
    public abstract class BaseModuleRewriter : IHttpModule
    {
        /// <summary>
        /// Executes when the module is initialized.
        /// </summary>
        /// <param name="app">A reference to the HttpApplication object processing this request.</param>
        /// <remarks>Wires up the HttpApplication's AuthorizeRequest event to the
        /// <see cref="BaseModuleRewriter_AuthorizeRequest"/> event handler.</remarks>
        public virtual void Init(HttpApplication app)
        {
            // WARNING!  This does not work with Windows authentication!
            // If you are using Windows authentication, change to app.BeginRequest
            app.AuthorizeRequest += new EventHandler(this.BaseModuleRewriter_AuthorizeRequest);
        }

        public virtual void Dispose() { }

        /// <summary>
        /// Called when the module's AuthorizeRequest event fires.
        /// </summary>
        /// <remarks>This event handler calls the <see cref="Rewrite"/> method, passing in the
        /// <b>RawUrl</b> and HttpApplication passed in via the <b>sender</b> parameter.</remarks>
        protected virtual void BaseModuleRewriter_AuthorizeRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            string url = app.Request.Url.AbsoluteUri;

            string houzui = "";
            string urltemp = url;//用来获取文件后缀
            int QuestionLastIndex = url.LastIndexOf('?');
            if (QuestionLastIndex>=0)
            {
                urltemp=url.Substring(0,QuestionLastIndex);
            }
            int lastindex = urltemp.LastIndexOf('.');
            if (lastindex > 0)
            {
                houzui = urltemp.Substring(lastindex);
            }

            //其他后缀的不进行伪静态处理
            string[] notInArray = new string[] { ".xml", ".asmx", ".ashx", ".css", ".js", ".eot", ".svg", ".ttf", ".woff", ".woff2", ".otf", ".jpg", ".png", ".gif", ".bmp", ".ico" };
            bool notIn = true;
            for (int i = 0; i < notInArray.Length; i++)
            {
                if (houzui.IndexOf(notInArray[i]) >= 0)
                {
                    notIn = false;
                    break;
                }
            }

            if (notIn)
            {
                if (houzui.IndexOf(".html") >= 0 || houzui.IndexOf(".htm") >= 0)
                {
                    //原本的静态文件不进行伪静态处理
                    string[] notStaticfileArray = new string[] { "/zjisuanji/", "/zkjcy/", "/zyjxfgcs/", "/zjszg/"};
                    bool NotStaticfile = true;
                    for (int i = 0; i < notStaticfileArray.Length; i++)
                    {
                        if (url.IndexOf(notStaticfileArray[i]) > 0)
                        {
                            NotStaticfile = false;
                            break;
                        }
                    }

                    if (NotStaticfile)
                    {
                        //Rewrite(app.Request.Path, app);
                        Rewrite(url, app); // ## ## ## 这里修改了     
                    }
                }
                else
                {
                    //Rewrite(app.Request.Path, app);
                    Rewrite(url, app); // ## ## ## 这里修改了
                }
            }
        }

        /// <summary>
        /// The <b>Rewrite</b> method must be overriden.  It is where the logic for rewriting an incoming
        /// URL is performed.
        /// </summary>
        /// <param name="requestedRawUrl">The requested RawUrl.  (Includes full path and querystring.)</param>
        /// <param name="app">The HttpApplication instance.</param>
        protected abstract void Rewrite(string requestedPath, HttpApplication app);
    }
}
