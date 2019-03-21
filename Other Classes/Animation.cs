using System;
using System.Threading;
using System.Windows.Forms;

namespace YourExperience
{
	 
	class Animation
	{
        //khai báo
        #region
            private Form form;
        #endregion

        //constructors
        #region
            public Animation(Form form)
		    {
                this.form = form;
                form.Opacity = 0d;
                Thread thread = new Thread(Run);
                thread.Name = "Animation_" + form.Name;
                thread.Start();
		    }
        #endregion

        private void Run()// y = sin(((x)^4) * π) lấy trong khoảng từ 0 đến 1.
        {
            try
			{
                for (int i = 0; i <= 30; i++)
                {
                    if(form.Opacity == 1d) return;
                    if (form.IsHandleCreated)
                    {
                        form.Invoke(new MethodInvoker(delegate ()
                        {
                            form.Opacity += Math.Sin(Math.Pow(i / 30d, 2) * Math.PI);
                        }));
                    }
                    Thread.Sleep(20);
                }
            }
			catch{ }			
		}
	}
}
