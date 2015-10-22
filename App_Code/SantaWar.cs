using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SantaWar
/// </summary>
public class SantaWar: NancyModule
{
	public SantaWar() : base("/santawar")
	{
        Post["/receivesecretphrase"] = _ => {
            PostSecretResponse secretPhrase = this.Bind<PostSecretResponse>();
            File.WriteAllText((HttpContext.Current.Server.MapPath("/secret") + "\\secretphrase.txt"), secretPhrase.secret);
            return secretPhrase;
        };

        Get["/receivesecretphrase"] = _ => "hola!";
	}

    internal class PostSecretResponse{
        public string secret {get; set;}
    }
}