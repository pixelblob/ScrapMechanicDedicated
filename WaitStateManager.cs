using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ScrapMechanicDedicated.GameStateManager;

namespace ScrapMechanicDedicated
{

    public delegate void Notify();  // delegate

    static class WaitStateManager
    {
        public static event Notify ThreadWaitStateChanged; // event

        public static void StartProcess()
        {
            Console.WriteLine("Thread State Watcher Started!");
            // some code here..
            //OnProcessCompleted();

            new Thread(new ThreadStart(threadStateWatcher)).Start();


        }

        public static void threadStateWatcher()
        {
            System.Diagnostics.ThreadState LastThreadState = proc.Threads[0].ThreadState;
            System.Diagnostics.ThreadWaitReason? LastWaitReason = null;
            while (proc != null)
            {
                if (proc.HasExited) break;
                //Debug.WriteLine("Checking Threads!");
                proc.Refresh();

                if (proc.Threads[0].ThreadState != LastThreadState)
                {
                    //Debug.WriteLine("Thread State Changed: " + proc.Threads[0].ThreadState);
                    LastThreadState = proc.Threads[0].ThreadState;

                    if (proc.Threads[0].ThreadState != System.Diagnostics.ThreadState.Wait && LastWaitReason != null)
                    {
                        LastWaitReason = null;
                        Debug.WriteLine("LAST WAIT REASON -> NULL");
                        OnThreadWaitStateChanged();
                    }

                        
                }



                if (proc.Threads[0].ThreadState == System.Diagnostics.ThreadState.Wait)
                {
                    if (proc.Threads[0].WaitReason != LastWaitReason && proc.Threads[0].WaitReason == System.Diagnostics.ThreadWaitReason.Suspended)
                    {
                        Debug.WriteLine("Wait State Changed: " + proc.Threads[0].WaitReason);
                        OnThreadWaitStateChanged();

                        LastWaitReason = proc.Threads[0].WaitReason;
                    }
                }
                else {
                    
                }
                Thread.Sleep(1000);
            }
        }

        static void OnThreadWaitStateChanged() //protected virtual method
        {
            //if ProcessCompleted is not null then call delegate
            ThreadWaitStateChanged?.Invoke();
        }
    }
}
