using Aspx_Demo.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Aspx_Demo
{
    public partial class Assign : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoDetailManager loDetailManager = new LoDetailManager();
                LoadLoDetails(loDetailManager);
            }
        }

        private void LoadLoDetails(LoDetailManager loDetailManager)
        {
            string loDetails = loDetailManager.GetLoDetail();
            lblLoDetails.Text = loDetails;
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            LoDetailManager loDetailManager = new LoDetailManager();
            string userName = txtUserName.Text;
            string result = loDetailManager.AddNewLoDetail(userName);
            lblLoDetails.Text = result;

            // Refresh the details after adding a user
            LoadLoDetails(loDetailManager);
        }

        protected void btnRemoveUser_Click(object sender, EventArgs e)
        {
            LoDetailManager loDetailManager = new LoDetailManager();
            int userId;
            if (int.TryParse(txtUserId.Text, out userId))
            {
                string result = loDetailManager.RemoveLoDetail(userId);
                lblLoDetails.Text = result;

                // Refresh the details after removing a user
                LoadLoDetails(loDetailManager);
            }
            else
            {
                lblLoDetails.Text = "Invalid UserId.";
            }
        }
    }
}
