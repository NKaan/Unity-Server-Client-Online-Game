#define ENABLE_UPDATE_FUNCTION_CALLBACK
#define ENABLE_LATEUPDATE_FUNCTION_CALLBACK
#define ENABLE_FIXEDUPDATE_FUNCTION_CALLBACK

using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class UnityThread : MonoBehaviour
{
    //(singleton) örneğimiz
   

    //UnityThread'i başlatmak için kullanılır. Herhangi bir fonksiyondan önce bir kere arayın.
    public static void initUnityThread(bool visible = false)
    {
        if (instance != null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            //sahneye görünmez bir oyun nesnesi ekle
            GameObject obj = new GameObject("MainThreadExecuter");
            if (!visible)
            {
                obj.hideFlags = HideFlags.HideAndDontSave;
            }

            DontDestroyOnLoad(obj);
            instance = obj.AddComponent<UnityThread>();
        }
    }

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

#if (ENABLE_UPDATE_FUNCTION_CALLBACK)

    public void Update()
    {
        if (noActionQueueToExecuteUpdateFunc)
        {
            return;
        }

        //ActionCopiedQueueUpdateFunc kuyruğundaki eski işlemleri temizleyin
        actionCopiedQueueUpdateFunc.Clear();
        lock (actionQueuesUpdateFunc)
        {
            //ActionQueuesUpdateFunc öğesini actionCopiedQueueUpdateFunc değişkenine kopyalayın
            actionCopiedQueueUpdateFunc.AddRange(actionQueuesUpdateFunc);
            //Şimdi kopyalamayı bitirdiğimizden beri actionQueuesUpdateFunc öğesini silin
            actionQueuesUpdateFunc.Clear();
            noActionQueueToExecuteUpdateFunc = true;
        }

        //ActionCopiedQueueUpdateFunc işlevlerini döngüden geçirin ve uygulayın
        for (int i = 0; i < actionCopiedQueueUpdateFunc.Count; i++)
        {
            actionCopiedQueueUpdateFunc[i].Invoke();
        }
    }

    private static UnityThread instance = null;

    //GÜNCELLEME IMPL
    //Başka bir Konudan alınan eylemleri tutar. ActionCopiedQueueUpdateFunc ile daha sonra başa çıkacak
    private static List<System.Action> actionQueuesUpdateFunc = new List<Action>();

    //actionQueuesUpdateFunc öğesinden kopyalanacak İşlemleri gerçekleştirilecek
    List<System.Action> actionCopiedQueueUpdateFunc = new List<System.Action>();
    // Used to know if whe have new Action function to execute. This prevents the use of the lock keyword every frame
    private volatile static bool noActionQueueToExecuteUpdateFunc = true;
    //GÜNCEL GÜNCELLEME IMPL
    //Yürütülecek yeni Eylem işlevinin olup olmadığını bilmek için kullanılır. Bu, her anahtar karede lock anahtar sözcüğünün kullanılmasını önler
    private static List<System.Action> actionQueuesLateUpdateFunc = new List<Action>();

    //GUNCELLEME IMPL
    public static void executeInUpdate(System.Action action)
    {
        if (action == null)
        {
            throw new ArgumentNullException("action");
        }

        lock (actionQueuesUpdateFunc)
        {
            actionQueuesUpdateFunc.Add(action);
            noActionQueueToExecuteUpdateFunc = false;
        }
    }

    public static void executeCoroutine(IEnumerator action)
    {
        if (instance != null)
        {
            executeInUpdate(() => instance.StartCoroutine(action));
        }
    }


#endif

    //actionQueuesLateUpdateFunc öğesinden kopyalanacak İşlemleri gerçekleştirilecek
    List<System.Action> actionCopiedQueueLateUpdateFunc = new List<System.Action>();

    // Used to know if whe have new Action function to execute. This prevents the use of the lock keyword every frame
    private volatile static bool noActionQueueToExecuteLateUpdateFunc = true;

    //SABİT GÜNCELLEME IMPL
    //Yürütülecek yeni Eylem işlevinin olup olmadığını bilmek için kullanılır. Bu, her anahtar karede lock anahtar sözcüğünün kullanılmasını önler
    private static List<System.Action> actionQueuesFixedUpdateFunc = new List<Action>();

    //holds Actions copied from actionQueuesFixedUpdateFunc to be executed
    List<System.Action> actionCopiedQueueFixedUpdateFunc = new List<System.Action>();

    //actionQueuesFixedUpdateFunc uygulamasından kopyalanan Eylemleri gerçekleştirilecek
    private volatile static bool noActionQueueToExecuteFixedUpdateFunc = true;

    //GÜNCEL GÜNCELLEME IMPL
#if (ENABLE_LATEUPDATE_FUNCTION_CALLBACK)



    public void LateUpdate()
    {
        if (noActionQueueToExecuteLateUpdateFunc)
        {
            return;
        }

        //Clear the old actions from the actionCopiedQueueLateUpdateFunc queue
        actionCopiedQueueLateUpdateFunc.Clear();
        lock (actionQueuesLateUpdateFunc)
        {
            //ActionCopiedQueueLateUpdateFunc kuyruğundaki eski işlemleri temizleyin
            actionCopiedQueueLateUpdateFunc.AddRange(actionQueuesLateUpdateFunc);
            //Şimdi kopyalamayı bitirdiğimizden beri actionQueuesLateUpdateFunc öğesini silin.
            actionQueuesLateUpdateFunc.Clear();
            noActionQueueToExecuteLateUpdateFunc = true;
        }

        //ActionCopiedQueueLateUpdateFunc işlevlerini döngüden geçirin ve uygulayın
        for (int i = 0; i < actionCopiedQueueLateUpdateFunc.Count; i++)
        {
            actionCopiedQueueLateUpdateFunc[i].Invoke();
        }


    }

    public static void executeInLateUpdate(System.Action action)
    {
        if (action == null)
        {
            throw new ArgumentNullException("action");
        }

        lock (actionQueuesLateUpdateFunc)
        {
            actionQueuesLateUpdateFunc.Add(action);
            noActionQueueToExecuteLateUpdateFunc = false;
        }
    }
#endif

    //SABİT GÜNCELLEME
#if (ENABLE_FIXEDUPDATE_FUNCTION_CALLBACK)

    public void FixedUpdate()
    {
        if (noActionQueueToExecuteFixedUpdateFunc)
        {
            return;
        }

        //Eski eylemleri actionCopiedQueueFixedUpdateFunc kuyruğundan temizleyin
        actionCopiedQueueFixedUpdateFunc.Clear();
        lock (actionQueuesFixedUpdateFunc)
        {
            //ActionQueuesFixedUpdateFunc öğesini actionCopiedQueueFixedUpdateFunc değişkenine kopyalayın
            actionCopiedQueueFixedUpdateFunc.AddRange(actionQueuesFixedUpdateFunc);
            //Şimdi kopyalamayı bitirdiğimizden beri actionQueuesFixedUpdateFunc öğesini silin.
            actionQueuesFixedUpdateFunc.Clear();
            noActionQueueToExecuteFixedUpdateFunc = true;
        }

        //ActionCopiedQueueFixedUpdateFunc işlevlerini döngüden geçirin ve uygulayın
        for (int i = 0; i < actionCopiedQueueFixedUpdateFunc.Count; i++)
        {
            actionCopiedQueueFixedUpdateFunc[i].Invoke();
        }
    }

    public static void executeInFixedUpdate(System.Action action)
    {
        if (action == null)
        {
            throw new ArgumentNullException("action");
        }

        lock (actionQueuesFixedUpdateFunc)
        {
            actionQueuesFixedUpdateFunc.Add(action);
            noActionQueueToExecuteFixedUpdateFunc = false;
        }
    }

   
#endif

    public void OnDisable()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
