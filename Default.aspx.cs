using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class secretphrase : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblSecretPhrase.Text = File.ReadAllText(Server.MapPath("/secret/secretphrase.txt"));
    }
}