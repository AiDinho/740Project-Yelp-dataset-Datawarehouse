using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Load_data
{
    public partial class Form1 : Form
    {
        Connection LoadModel;

        public Form1()
        {
            
            InitializeComponent();
            lblProcess.Text = "";
            LoadModel = new Connection();
        }

        private void ShowFBDPatsh(object sender, EventArgs e)
        {
            if(FBDpatsh.ShowDialog()==DialogResult.OK)
            {
                TBPath.Text = FBDpatsh.SelectedPath;
            }
        }

        private void LoadData(object sender, EventArgs e)
        {
            if (TBPath.Text == null || TBPath.Text == "")
            {
                MessageBox.Show("No path enter");
            }
            else
            {
                bool a = LoadModel.FilesExists(TBPath.Text);
                if (!a)
                {
                    MessageBox.Show("Some file dose not exits: \n yelp_academic_dataset_review.JSON \n yelp_academic_dataset_tip.JSON  \n yelp_academic_dataset_checkin.JSON \n yelp_academic_dataset_user.JSON \n yelp_academic_dataset_business.JSON \n photo_id_to_business_id.JSON");

                }
                else
                {
                    string rez = LoadModel.ConnectToDB();

                    if (rez == null || rez == "")
                    {
                        

                        //Load_with_text(1, "MODEL IS DOWNLOADING...\n Review base ");

                        Load_with_text(2, "MODEL IS DOWNLOADING...\n Tip base ");

                        Load_with_text(3, "MODEL IS DOWNLOADING...\n Buisiness base ");

                        Load_with_text(4, "MODEL IS DOWNLOADING...\n USER base ");

                        Load_with_text(5, "MODEL IS DOWNLOADING...\n Checkin base ");

                        Load_with_text(1, "MODEL IS DOWNLOADING...\n Review base ");

                        Load_with_text(6, "MODEL IS DOWNLOADING...\n Optimization of Database ");

                        lblProcess.Text = "";
                        lblProcess.Refresh();
                        MessageBox.Show("Your files was downloaded");

                    }
                    else
                    {
                        MessageBox.Show(rez);
                    }
                }
               
            }
        }


        private void Load_with_text(int a, string text)
        {
            lblProcess.Text = text;
            lblProcess.Refresh();
            LoadModel.Start_Load(TBPath.Text, a);

        }

    }
}
