using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;

namespace DxlibCs
{
    class Fps{
	    int mStartTime;         //測定開始時刻
	    int mCount;             //カウンタ
	    float mFps;             //fps
	    static int N = 60;//平均を取るサンプル数
	    static int FPS = 60;	//設定したFPS

        public Fps(){
	    	mStartTime = 0;
		    mCount = 0;
		    mFps = 0;
	    }

    	public bool Update(){
	    	if( mCount == 0 ){ //1フレーム目なら時刻を記憶
		    	mStartTime = DX.GetNowCount();
		    }
		    if( mCount == N ){ //60フレーム目なら平均を計算する
			    int t = DX.GetNowCount();
			    mFps = 1000/((t-mStartTime)/(float)N);
			    mCount = 0;
			    mStartTime = t;
		    }
		    mCount++;
		    return true;
	    }

    	public void Draw(){
	    	int w = 640 , h = 480;
		    DX.DrawString( w - 120 , h - 20, mFps.ToString() , DX.GetColor( 255 , 255 , 255 )  );
	    }

    	public void Wait(){
	    	int tookTime = DX.GetNowCount() - mStartTime;	//かかった時間
		    int waitTime = mCount*1000/FPS - tookTime;	//待つべき時間
		    if( waitTime > 0 ){
                System.Threading.Thread.Sleep(waitTime);
    		}
	    }
    }

    class main
    {

        
        [STAThread]
        static void Main()
        {
            // ウィンドウモードに切り替え
            DX.ChangeWindowMode(DX.TRUE);

            // ウィンドウのサイズ指定
            DX.SetGraphMode(800, 600, 32);

            // DXライブラリの初期化
            if (DX.DxLib_Init() == -1)
            {
                // 初期化に失敗した場合は終了
                return;
            }

            int time = 0;
            var fps = new Fps();

            // 描画先を裏画面にする
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);

            Console.WriteLine("ぬわああああ");
            while (DX.ProcessMessage() == 0 && DX.ClearDrawScreen() == 0){
                fps.Update();	//計算

                DX.DrawBox(10 , 10 , 470 , 590 , DX.GetColor(128 , 128 , 255 ) , 1);

                function.AlphaSet(128 + time %128);
                DX.DrawString(100, 200, "DxLib for C#", DX.GetColor(255, 255, 64));
                DX.DrawString(100, 400, time.ToString() , DX.GetColor(255, 255, 64));
                function.AlphaSet();
                ++time;
                if(time % 60 == 60)
                    Console.WriteLine(time);

                fps.Draw();		//描画




                DX.clsDx();
                DX.ScreenFlip();

                fps.Wait();	

            }

            // キー入力待ち
            DX.WaitKey();

            // DXライブラリの使用終了
            DX.DxLib_End();

            return;
        }
    }

    /// <summary>
    /// FPS制御用クラス
    /// </summary>
    class FPSTimer
    {
        // フレーム時刻の基準となる時刻（単位：ms）
        // System.Environment.TickCount により取得
        int baseTickCount = 0;

        // 前回のフレームの時刻（単位：µs）
        // baseTickCountからの差分で表す
        int prevTickCount = 0;

        // 現在のフレームの時刻（単位：μs）
        // baseTickCountからの差分で表す
        int nowTickCount = 0;

        // 1フレームの時間（単位：μs）
        // デフォルト値＝60FPS≒1.66μs
        int period = 1000 * 1000 / 60;

        // 目標のFPS
        // デフォルト値＝60FPS
        int fps = 60;

        // 計測した（実際の）FPS
        int fpsReal = 0;

        // FPS計測用のカウンタ値
        int fpsCount = 0;

        // FPS測定用の時刻（単位：ms）
        int fpsTickCount = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fps">目標のFPS（省略時は60FPS）</param>
        public FPSTimer(int fps = 60)
        {
            // 基準となる時刻を取得
            baseTickCount = System.Environment.TickCount;

            // FPSをセット
            this.fps = fps;

            // 1フレームの時間をセット
            period = 1000 * 1000 / fps;
        }

        /// <summary>
        /// 次のフレームを待つ。ゲームループの先頭で呼び出すことを期待している。
        /// </summary>
        /// <returns>常にtrueを返す（将来拡張用）</returns>
        public bool WaitNextFrame()
        {
            // フレームの時刻を更新する
            prevTickCount += period;

            // 基準となる時刻からの差分を求める。
            nowTickCount = (System.Environment.TickCount - baseTickCount) * 1000;

            // 次のフレームまで到達しているか？
            if (nowTickCount >= (prevTickCount + period))
            {
                return true;
            }

            // 次のフレームまで到達していなければ、スリープする
            while (nowTickCount < (prevTickCount + period))
            {
                System.Threading.Thread.Sleep(1);
                nowTickCount = (System.Environment.TickCount - baseTickCount) * 1000;
            }

            return true;
        }

#if false // 現在使わない
        /// <summary>
        /// 次のフレームが来ているかどうかを判定する。
        /// </summary>
        /// <returns>true:次のフレームまで到達している false:次のフレームまで到達していない</returns>
        public bool IsNextFrame()
        {
            // 基準となる時刻からの差分を求める。
            nowTickCount = (System.Environment.TickCount - baseTickCount) * 1000;

            // 次のフレームまで到達しているか？
            if (nowTickCount >= (prevTickCount + period))
            {
                return true;
            }

            // 次のフレームまで到達していない
            return false;
        }
#endif

#if false // 現在使わない
        /// <summary>
        /// 次のフレームへの移動。ゲームループの末尾で呼び出すことを期待している
        /// </summary>
        public void NextFrame()
        {
            // フレームの時刻を更新する
            prevTickCount += period;
        }
#endif

        /// <summary>
        /// このフレームの描画が行えるかどうかを返す。
        /// 現在の時刻と次のフレームの時刻の差から描画する余裕があれば ture，
        /// 余裕がなければ false を返す
        /// </summary>
        /// <returns>true:描画可能 false:描画不可</returns>
        public bool IsDraw()
        {
            // 基準となる時刻からの差分を求める。
            nowTickCount = (System.Environment.TickCount - baseTickCount) * 1000;

            // 描画する時間がある場合は true:描画可能 を返す
            if (nowTickCount < (prevTickCount + period * 2))
            {
                return true;
            }

            // false:描画不可 を返す
            return false;
        }

        /// <summary>
        /// FPSを計算する。描画を実施したら呼ぶことを期待している。
        /// FPSを計測しない場合は呼ぶ必要はない。
        /// </summary>
        public void CalcFps()
        {
            // カウンタを増やす
            fpsCount++;

            // 前回の計測時刻から1秒以上経過していれば、フレームレートを計算
            int tickCount = System.Environment.TickCount;
            if (tickCount - fpsTickCount >= 1000)
            {
                fpsReal = (fpsCount * 1000) / (tickCount - fpsTickCount);
                fpsTickCount = tickCount;
                fpsCount = 0;
            }
        }

        /// <summary>
        /// FPS（目標値）を取得する
        /// </summary>
        /// <returns>FPS（目標値）</returns>
        public int GetFps()
        {
            return fps;
        }

        /// <summary>
        /// FPS（計測値）を取得する
        /// </summary>
        /// <returns>FPS（計測値）</returns>
        public int GetFpsReal()
        {
            return fpsReal;
        }

        
    }

}