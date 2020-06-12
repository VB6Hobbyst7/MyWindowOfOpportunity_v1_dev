using System;
using System.Web.UI.WebControls;
using Examine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UmbracoExamine;

namespace usercontrols.Umbraco
{
    public partial class ExamineIndexAdmin : System.Web.UI.UserControl
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                InitRepeater();
            }

        }

        private void InitRepeater()
        {
            indexManager.DataSource = ExamineManager.Instance.IndexProviderCollection;
            indexManager.DataBind();
            
        }

        protected void RebuildClick(Object sender,EventArgs e)
        {
            Button clickedButton = (Button)sender;
            string indexToRebuild = clickedButton.CommandArgument;
            ExamineManager.Instance.IndexProviderCollection[indexToRebuild].RebuildIndex();
            result.Text = string.Format("Index {0} added to index queue", indexToRebuild);
            result.Visible = true;
        }


        public void ExamineIndexRebuild(Object sender, EventArgs e)
        {
            //// Rebuild selected indexes
            //var indexes = new List<string> { "InternalMemberIndexer", "InternalIndexer", "ExternalIndexer" }; 
            //indexes.ForEach(index => ExamineManager.Instance.IndexProviderCollection[index].RebuildIndex());

            //Or rebuild them all
            ExamineManager.Instance.IndexProviderCollection.ToList().ForEach(index => index.RebuildIndex());
            
            //'Response.Write("<h3>Rebuilt</h3>");
        }

        private XElement GetMediaItem(int nodeId)
        {
            var nodes = umbraco.library.GetMedia(nodeId, false);
            return XElement.Parse(nodes.Current.OuterXml);
        }

        public void ClearSession(Object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            //'Response.Write("<h3>Session Cleared!!!</h3>");
        }
    }
}