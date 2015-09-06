﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ITcinema
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            randomVideo();
            if(!IsPostBack)
                fillSlider();
        }

        public void fillSlider()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["Konekcija"].ConnectionString;
            string sqlstring = "SELECT * FROM Movie";
            SqlCommand komanda = new SqlCommand(sqlstring, conn);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = komanda;
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                adapter.Fill(ds, "Movies");
                lview.DataSource = ds.Tables["Movies"];
                lview.DataBind();
            }
            catch(Exception err)
            {

            }
            finally
            {
                conn.Close();
            }
        }

        public void randomVideo()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["Konekcija"].ConnectionString;
            string sqlstring = "SELECT * FROM Movie";
            SqlCommand komanda = new SqlCommand(sqlstring, conn);
            try
            {
                conn.Open();
                SqlDataReader reader = komanda.ExecuteReader();
                ArrayList filmovi = new ArrayList();
                while(reader.Read())
                {
                    string url = reader["URL"].ToString();
                    string videoID = url.Substring(url.IndexOf('=') + 1);
                    filmovi.Add(videoID);
                }
                Random r = new Random();
                iframe.Attributes.Add("src", "https://www.youtube.com/embed/" + filmovi[r.Next(filmovi.Count)]);
                reader.Close();
            }
            catch (Exception err)
            {
                error.Text = err.ToString();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}