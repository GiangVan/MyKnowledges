using System;
using System.Threading;
using System.Windows.Forms;

namespace YourExperience
{
	 
	class AnimationMOVE
	{
        //khai báo
        #region
            private int index;
            private bool is_increase;
            private bool is_vertical = new bool();
            private Control control;
            private int speedTime = 10;
            private string HashCode;//dùng để nhận biết xem control có đung được AnimationMOVE hay không, nếu control.Name == HashCode là có và ngược lại
            private int delayTime = 0;
        #endregion

        //constructors
        #region
            public AnimationMOVE(Control control, int index, bool is_increase)
		    {
                //control.Enabled = false;
                this.is_increase = is_increase;
                this.index = index;
			    this.control = control;
                HashCode = control.GetHashCode().ToString() + GetHashCode().ToString();
                control.Name = HashCode;
                Thread thread = new Thread(Run);
                //thread.Name = "AnimationMOVE (" + GetHashCode() + ")";
                thread.Start();
		    }
            public AnimationMOVE(Control control, int index, bool is_increase,bool is_vertical, int delayTime, int speedTime)
            {
            //control.Enabled = false;
            this.speedTime = speedTime;
            this.delayTime = delayTime;
            this.is_vertical = is_vertical;
            this.is_increase = is_increase;
            this.index = index;
            this.control = control;
            HashCode = control.GetHashCode().ToString() + GetHashCode().ToString();
            control.Name = HashCode;
            Thread thread = new Thread(Run);
            //thread.Name = "AnimationMOVE (" + GetHashCode() + ")";
            thread.Start();
        }
        #endregion

        private void Run()// y = sin(((x-1)^4) * π) lấy trong khoảng từ 0 đến 1.
        {
            if (delayTime > 0) Thread.Sleep(delayTime);
            if (control.Name != HashCode) return; //nếu control này được AnimationMOVE ở một luồng khác thì out
            try//khi chương trình đóng mà form Info vẫn chưa bật thì sẽ đóng form info nhưng form lại gọi hàm đóng SETTING nên sẽ gây ra lỗi vì formMain lúc đó đã đóng rồi.
			{
                double speed = new double();
				double extraVAR;
				int n = 20;//sô lần lặp.

                if(is_vertical)
                    extraVAR = (index - control.Top) / 5.98;//biến phụ.
                else
                    extraVAR = (index - control.Left) / 6.3;//biến phụ.

                for (int i = 0; i < n; i++)
                {
                    
                    if (control.Name != HashCode) return; //nếu control này được AnimationMOVE ở một luồng khác thì out

                    if(is_increase || (is_vertical && !is_increase))
                        speed = extraVAR * (Math.Sin(Math.Pow((i / (double)n) - 1, 4) * Math.PI));//sài tạm với n=20.
                    else
                        speed = extraVAR * (Math.Sin(Math.Pow((i / (double)n) - 0, 4) * Math.PI));//sài tạm với n=20.
                    control.Invoke(new MethodInvoker(delegate ()
                    {
                        if (is_vertical)
                            control.Top += (int)speed;
                        else
                            control.Left += (int)speed;
                    }));

                    Thread.Sleep(speedTime);
                }

                control.Invoke(new MethodInvoker(delegate ()
                {
                    if (is_vertical)
                        control.Top = index;
                    else
                        control.Left = index;
                    //control.Enabled = true;
                    //if (is_increase && is_vertical) MessageBox.Show(control.Top.ToString());
                }));
            }
			catch { }				
		}
	}
}
