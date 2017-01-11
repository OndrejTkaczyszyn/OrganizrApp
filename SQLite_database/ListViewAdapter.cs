using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite_database.Model;

namespace SQLite_database
{

    public class ViewHolder : Java.Lang.Object
    {
        public TextView txtName { get; set; }
        public TextView txtAge { get; set; }
        public TextView txtEmail { get; set; }
    }
   public class ListViewAdapter:BaseAdapter
    {
        private Activity activity;
        private List<Task> listTask;

        public ListViewAdapter(Activity activity, List<Task> listTask)
        {
            this.activity = activity;
            this.listTask = listTask;
        }

        public override int Count
        {
            get
            {
                return listTask.Count;
            }
            
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return listTask[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.list_view_data_template, parent, false);

            var txtName = view.FindViewById<TextView>(Resource.Id.textName);//SSName
            var txtText = view.FindViewById<TextView>(Resource.Id.textText);//SSText
            var txtDate = view.FindViewById<TextView>(Resource.Id.textDate);//SSDate

            //set text to each corresponding item
            txtName.Text = listTask[position].Name;//SS
            txtDate.Text = ""+listTask[position].Date;//SS
            txtText.Text = listTask[position].Text;//SS

            return view;
        }
    }
}