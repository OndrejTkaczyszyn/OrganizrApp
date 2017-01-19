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
    static class Constants
    {
        public const int ASSOC_DATABASE_ID = 1;
    }
    
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

            view.SetTag(Resource.Id.editLayout, GetItemId(position));//set the tag so that we can later edit database item with that id
            /*view.Tag = GetItemId(position);*/
            var txtName = view.FindViewById<TextView>(Resource.Id.textName);//SSName
             var txtText = view.FindViewById<TextView>(Resource.Id.textText);//SSText
            var txtDate = view.FindViewById<TextView>(Resource.Id.textDate);//SSDate
            var txtReminder1 = view.FindViewById<TextView>(Resource.Id.reminderLine1);
            var txtReminder2 = view.FindViewById<TextView>(Resource.Id.reminderLine2);
            //set text to each corresponding item
            txtName.Text = listTask[position].Name;//SS
            txtDate.Text = ""+listTask[position].Id;//SS
            txtText.Text = listTask[position].Text;//SS
            txtReminder1.Text = String.Concat(listTask[position].Date.Day,"/",listTask[position].Date.Month,"/",listTask[position].Date.Year);
            txtReminder2.Text = String.Concat(listTask[position].Date.Hour, " : ", listTask[position].Date.Minute);

            
           

            txtText.Visibility = ViewStates.Gone;

            return view;
        }
    }
}