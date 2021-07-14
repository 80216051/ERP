using System;
using System.Web;
using System.Web.Caching;
using System.Configuration;
using System.Xml.Serialization;
using System.Xml;
using BaseLibrary.Tool;

namespace Utility.URLRewriter
{
    /// <summary>
    /// Specifies the configuration settings in the Web.config for the RewriterRule.
    /// </summary>
    /// <remarks>This class defines the structure of the Rewriter configuration file in the Web.config file.
    /// Currently, it allows only for a set of rewrite rules; however, this approach allows for customization.
    /// For example, you could provide a ruleset that <i>doesn't</i> use regular expression matching; or a set of
    /// constant names and values, which could then be referenced in rewrite rules.
    /// <p />
    /// The structure in the Web.config file is as follows:
    /// <code>
    /// &lt;configuration&gt;
    /// 	&lt;configSections&gt;
    /// 		&lt;section name="RewriterConfig" 
    /// 		            type="URLRewriter.Config.RewriterConfigSerializerSectionHandler, URLRewriter" /&gt;
    ///		&lt;/configSections&gt;
    ///		
    ///		&lt;RewriterConfig&gt;
    ///			&lt;Rules&gt;
    ///				&lt;RewriterRule&gt;
    ///					&lt;LookFor&gt;<i>pattern</i>&lt;/LookFor&gt;
    ///					&lt;SendTo&gt;<i>replace with</i>&lt;/SendTo&gt;
    ///				&lt;/RewriterRule&gt;
    ///				&lt;RewriterRule&gt;
    ///					&lt;LookFor&gt;<i>pattern</i>&lt;/LookFor&gt;
    ///					&lt;SendTo&gt;<i>replace with</i>&lt;/SendTo&gt;
    ///				&lt;/RewriterRule&gt;
    ///				...
    ///				&lt;RewriterRule&gt;
    ///					&lt;LookFor&gt;<i>pattern</i>&lt;/LookFor&gt;
    ///					&lt;SendTo&gt;<i>replace with</i>&lt;/SendTo&gt;
    ///				&lt;/RewriterRule&gt;
    ///			&lt;/Rules&gt;
    ///		&lt;/RewriterConfig&gt;
    ///		
    ///		&lt;system.web&gt;
    ///			...
    ///		&lt;/system.web&gt;
    ///	&lt;/configuration&gt;
    /// </code>
    /// </remarks>
    [Serializable()]
    [XmlRoot("RewriterConfig")]
    public class RewriterConfiguration
    {
        // private member variables
        private RewriterRuleCollection rules;			// an instance of the RewriterRuleCollection class...

        /// <summary>
        /// GetConfig() returns an instance of the <b>RewriterConfiguration</b> class with the values populated from
        /// the Web.config file.  It uses XML deserialization to convert the XML structure in Web.config into
        /// a <b>RewriterConfiguration</b> instance.
        /// </summary>
        /// <returns>A <see cref="RewriterConfiguration"/> instance.</returns>
        public static RewriterConfiguration GetConfig()
        {
            //RewriterConfiguration dd = GetRewriterInfo();
            //if (HttpContext.Current.Cache["RewriterConfig"] == null)
            //    HttpContext.Current.Cache.Insert("RewriterConfig", ConfigurationSettings.GetConfig("RewriterConfig"));

            //return (RewriterConfiguration)HttpContext.Current.Cache["RewriterConfig"];
            return GetRewriterInfo();
        }

        #region ��ȡα��̬����
        private static RewriterConfiguration GetRewriterInfo()
        {
            RewriterConfiguration rewriter = new RewriterConfiguration();
            string urlconfig = CommonTool.GetMapPath("/config/URLRewriter.config");
            string CacheKey = "URLHtmlConfigWriterCache";
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            if (objCache[CacheKey] == null)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(urlconfig);
                XmlElement root = doc.DocumentElement;
                RewriterRuleCollection rules = new RewriterRuleCollection();
                XmlNodeList items = root.SelectSingleNode("/RewriterConfig/Rules").ChildNodes;
                foreach (XmlNode item in items)
                {
                    XmlNodeList sonItem = item.ChildNodes;
                    //������滹�нڵ㣬˵����������Ϣ����������ע�͵���Ϣ
                    if (sonItem.Count > 0)
                    {
                        RewriterRule rule = new RewriterRule();
                        rule.LookFor = item["LookFor"].InnerText;
                        rule.SendTo = item["SendTo"].InnerText;
                        rules.Add(rule);
                    }
                }
                rewriter.Rules = rules;

                if (rewriter!=null&&rewriter.rules!=null)
                {
                    objCache.Insert(CacheKey, rewriter);
                }

                return rewriter;
            }
            else
            {
                return (RewriterConfiguration)objCache[CacheKey];
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// A <see cref="RewriterRuleCollection"/> instance that provides access to a set of <see cref="RewriterRule"/>s.
        /// </summary>
        public RewriterRuleCollection Rules
        {
            get
            {
                return rules;
            }
            set
            {
                rules = value;
            }
        }
        #endregion
    }
}
