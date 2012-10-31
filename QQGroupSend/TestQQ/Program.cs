using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Format.WebQQ.WebQQ2.DLL;
using System.IO;
using System.Drawing;
using System.Threading;
using Format.WebQQ.Model.Messages;


namespace Format.WebQQ.TestQQ
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //Robot.ConfigManger manager = new Robot.ConfigManger();
            //manager.Save();

            //using (QQDbContext db = new QQDbContext())
            //{
                //GroupMessage m2 = null;

                //for (int i = 0; i < 10; i++)
                //{
                //    m2 = new GroupMessage { ID = i, GroupUin = 123L, Message = "test group message" };
                //    db.GroupMessages.Add(m2);

                //    db.JoinGroupRequestMessages.Add(
                //        new JoinGroupRequestMessage
                //        {
                //            ID = i,
                //            GroupUin = "G" + i,
                //            RequestMessage = "Join",
                //            RequestUin = String.Format("Q{0}", i)
                //        });
                //}

                //db.SaveChanges();
            //}

            using (WinLogin winLogin = new WinLogin())
            {
                winLogin.ShowDialog();
            }
        }


    }
}
