using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class isMyball : MonoBehaviour
{

    struct data
    {
        public Vector3 pos;
        public Vector3 vel;
        public float mass;
    };

    public Camera cam;
    public enum MODE { SQUARE, BUNNY, BUDDHA,ARMA, DINO, A, O};
    public MODE mode;
    public Shader rainbowShader;
    public Shader whiteShader;
    public ComputeShader cShader;
    public int squareIndex;
    public GameObject sphere;
    private data[] value;
    private Material mat;
    private ComputeBuffer cBuff = null;
    private int kernelHandle;
    private int width, height, depth;
    private int particleSize;
    private int masterCount;
    private double dts;
    
    private int[] mapper = { 32, 64, 128, 256, 512, 1024, 2048 };


    float scaler =1.0f;
    // Use this for initialization
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 9999;
        init();
    }
    private void OnDisable()
    {
        //cBuff.Release();
    }
    void initSquare()
    {
        width = mapper[squareIndex];
        height = mapper[squareIndex];
        depth = mapper[squareIndex];
        particleSize = width * height * depth;
        value = new data[particleSize];
        for (int i = 0; i < particleSize; i++)
        {
            value[i].pos = new Vector3(0, 0, 0);
            value[i].vel = new Vector3(0, 0, 0);
        }
        float dx = 1.0f / (1.0f * width), dy = 1.0f / (1.0f * height), dz = 1.0f / depth;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < depth; k++)
                {
                    value[i * height + j + (width * height) * k].pos.x = (dx * i) - 0.5f;
                    value[i * height + j + (width * height) * k].pos.y = dy * j;
                    value[i * height + j + (width * height) * k].pos.z = k * dz-0.5f;
                    value[i * height + j + (width * height) * k].mass = 1.0f;
                }
            }
        }
    }
    void initBunny()
    {
        StreamReader sr = new StreamReader("Assets/bunny.txt");
        string temp = sr.ReadLine();
        string[] tempArr = temp.Split(new char[] { ' ' });

        GameObject gb = new GameObject();

        particleSize = int.Parse(tempArr[0]);
        value = new data[particleSize];
        for (int i = 0; i < particleSize; i++)
        {
            temp = sr.ReadLine();
            tempArr = temp.Split(new char[] { ' ' });

            value[i].pos.x = float.Parse(tempArr[1]) / scaler;
            value[i].pos.y = float.Parse(tempArr[2]) / scaler;
            value[i].pos.z = float.Parse(tempArr[3]) / scaler;
            value[i].pos.y += 1.0f;
            //gb.transform.position.Set(value[i].pos.x,
            //    value[i].pos.y,
            //    value[i].pos.z);
            //gb.transform.Rotate(new Vector3(1, 1, 0), 45);

            //value[i].pos.x = gb.transform.position.x;
            //value[i].pos.y = gb.transform.position.x;
            //value[i].pos.z = gb.transform.position.x;
            //value[i].pos = Quaternion.Euler(225.0f, 200.0f, 0.0f) * value[i].pos;
            value[i].mass = 1.0f;
        }
        
        //Debug.Log(gb.transform.position.x);
        //Debug.Log(value[0].pos.x + " " + value[0].pos.y + " " + value[0].pos.z);
    }
    void initDino()
    {
        StreamReader sr = new StreamReader("Assets/dino.txt");
        string temp = sr.ReadLine();
        string[] tempArr = temp.Split(new char[] {' '});

        GameObject gb = new GameObject();

        particleSize = int.Parse(tempArr[0]);
        value = new data[particleSize];
        for (int i = 0; i < particleSize; i++)
        {
            temp = sr.ReadLine();
            tempArr = temp.Split(new char[] { ' ' });

            value[i].pos.x = float.Parse(tempArr[1]) / scaler;
            value[i].pos.y = float.Parse(tempArr[2]) / scaler;
            value[i].pos.z = float.Parse(tempArr[3]) / scaler;
            value[i].pos.y -= 2.0f;
            //gb.transform.position.Set(value[i].pos.x,
            //    value[i].pos.y,
            //    value[i].pos.z);
            //gb.transform.Rotate(new Vector3(1, 1, 0), 45);
            //value[i].pos = Quaternion.Euler(225.0f, 200.0f, 0.0f) * value[i].pos;
            value[i].mass = 1.0f;
        }
        //Debug.Log(gb.transform.position.x);
        
        Debug.Log(value[0].pos.x + " " + value[0].pos.y + " " + value[0].pos.z);
    }
    void initArma()
    {
        StreamReader sr = new StreamReader("Assets/armadilo.txt");
        string temp = sr.ReadLine();
        temp.Trim();
        
        string[] tempArr = temp.Split(new char[] { ' ' });

        GameObject gb = new GameObject();

        particleSize = int.Parse(tempArr[0]);
        value = new data[particleSize];
        for (int i = 0; i < particleSize; i++)
        {

            temp = sr.ReadLine();
            temp =temp.Trim();
            tempArr = temp.Split(new char[] { ' ' });
            //for(int j=0;j<tempArr.Length;j++)
            //{
            //    Debug.Log("index"+j+"="+tempArr[j]);
            //}
            value[i].pos.x = float.Parse(tempArr[4]) / scaler;
            value[i].pos.y = float.Parse(tempArr[6]) / scaler;
            value[i].pos.z = float.Parse(tempArr[8]) / scaler;
            gb.transform.position.Set(value[i].pos.x,
                value[i].pos.y,
                value[i].pos.z);
            gb.transform.Rotate(new Vector3(1, 1, 0), 45);

            //value[i].pos.x = gb.transform.position.x;
            //value[i].pos.y = gb.transform.position.x;
            //value[i].pos.z = gb.transform.position.x;
            //value[i].pos = Quaternion.Euler(225.0f, 200.0f, 135.0f) * value[i].pos;
            
            value[i].mass = 1.0f;
        }
        //Debug.Log(gb.transform.position.x);
        Debug.Log(value[0].pos.x + " " + value[0].pos.y + " " + value[0].pos.z);
    }
    void initBuddha()
    {
        StreamReader sr = new StreamReader("Assets/buddha.txt");
        string temp = sr.ReadLine();
        string[] tempArr = temp.Split(new char[] { ' ' });

        GameObject gb = new GameObject();
        int dummy;
        
        particleSize = int.Parse(tempArr[0]);
        dummy = int.Parse(tempArr[1]);
        dummy = int.Parse(tempArr[2]);
        dummy = int.Parse(tempArr[3]);
        value = new data[particleSize];
        for (int i = 0; i < particleSize; i++)
        {
            temp = sr.ReadLine();
            tempArr = temp.Split(new char[] {' ',' '});

            value[i].pos.x = (float)double.Parse(tempArr[1]) / scaler;
            value[i].pos.y = (float)double.Parse(tempArr[2]) / scaler;
            value[i].pos.z = (float)double.Parse(tempArr[3]) / scaler;
            //float.TryParse(tempArr[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CreateSpecificCulture("ko-KR"),out value[i].pos.x);
            //float.TryParse(tempArr[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CreateSpecificCulture("ko-KR"), out value[i].pos.y);
            //float.TryParse(tempArr[3], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CreateSpecificCulture("ko-KR"), out value[i].pos.z);

            //value[i].pos.x /= scaler;
            //value[i].pos.y /= scaler;
            //value[i].pos.z /= scaler;
            //gb.transform.position.Set(value[i].pos.x,value[i].pos.y,value[i].pos.z);
            //gb.transform.Rotate(new Vector3(1, 1, 0), 45);

            //value[i].pos.x = gb.transform.position.x;
            //value[i].pos.y = gb.transform.position.x;
            //value[i].pos.z = gb.transform.position.x;
            //value[i].pos = Quaternion.Euler(225.0f, 200.0f, 0.0f) * value[i].pos;
            
            value[i].mass = 1.0f;
        }
        //Debug.Log(gb.transform.position.x);
        Debug.Log(value[0].pos.x + " " + value[0].pos.y + " " + value[0].pos.z);
    }
    void initA()
    {
        StreamReader sr = new StreamReader("Assets/A.txt");
        string temp = sr.ReadLine();
        string[] tempArr = temp.Split(new char[] { ' ' });

        GameObject gb = new GameObject();
        int dummy;

        particleSize = int.Parse(tempArr[0]);
        dummy = int.Parse(tempArr[1]);
        dummy = int.Parse(tempArr[2]);
        dummy = int.Parse(tempArr[3]);
        value = new data[particleSize];
        //Debug.Log("particleSize = " + particleSize);
        for (int i = 0; i < particleSize; i++)
        {
            temp = sr.ReadLine();
            tempArr = temp.Split(new char[] { ' '});
            //Debug.Log("data= " + tempArr[2]);
            value[i].pos.x = float.Parse(tempArr[2]) / scaler;
            value[i].pos.y = float.Parse(tempArr[3]) / scaler;
            value[i].pos.z = float.Parse(tempArr[4]) / scaler;
            value[i].mass = 1.0f;
            value[i].pos.y -= 0.1f;
        }
        //Debug.Log(gb.transform.position.x);
        Debug.Log(value[0].pos.x + " " + value[0].pos.y + " " + value[0].pos.z);
    }
    void initO()
    {
        StreamReader sr = new StreamReader("Assets/O.txt");
        string temp = sr.ReadLine();
        string[] tempArr = temp.Split(new char[] { ' ' });

        GameObject gb = new GameObject();
        int dummy;

        particleSize = int.Parse(tempArr[0]);
        dummy = int.Parse(tempArr[1]);
        dummy = int.Parse(tempArr[2]);
        dummy = int.Parse(tempArr[3]);
        value = new data[particleSize];
        //Debug.Log("particleSize = " + particleSize);
        for (int i = 0; i < particleSize; i++)
        {
            temp = sr.ReadLine();
            tempArr = temp.Split(new char[] { ' ' });
            //Debug.Log("data= " + tempArr[2]);
            value[i].pos.x = float.Parse(tempArr[2]) / scaler;
            value[i].pos.y = float.Parse(tempArr[3]) / scaler;
            value[i].pos.z = float.Parse(tempArr[4]) / scaler;
            value[i].mass = 1.0f;
        }
        //Debug.Log(gb.transform.position.x);
        Debug.Log(value[0].pos.x + " " + value[0].pos.y + " " + value[0].pos.z);
    }
    void initCam()
    {
        //cam.transform.position = new Vector3(3, 2, 5);
        cam.transform.LookAt(new Vector3(0, 0, 0), new Vector3(0, 1, 0));
    }
    void init()
    {
        initCam();
        //other
        mat = new Material(rainbowShader);
        switch (mode)
        {
            case MODE.SQUARE:
                
                initSquare();
                break;
            case MODE.BUNNY:
                scaler = 20.0f;
                initBunny();
                break;
            case MODE.BUDDHA:
                scaler = 0.05f;
                initBuddha();
                break;
            case MODE.ARMA:
                scaler = 10.0f;
                initArma();
                break;
            case MODE.DINO:
                scaler = 50.0f;
                initDino();
                break;
            case MODE.A:
                scaler = 50.0f;
                initA();
                break;
            case MODE.O:
                scaler = 50.0f;
                initO();
                break;
        }
        if (cBuff != null)
        {
            cBuff.Release();
        }
        
        Debug.Log("particleSize = "+particleSize);
        cBuff = new ComputeBuffer(particleSize, 28);
        mat.SetFloat("x", (float)width);
        mat.SetFloat("y", (float)height);
        mat.SetFloat("z", (float)width);

        kernelHandle = cShader.FindKernel("CSMain");
        cBuff.SetData(value);
        mat.SetBuffer("value", cBuff);
        cShader.SetBuffer(kernelHandle, "value", cBuff);
        cShader.SetVector("sphere1",sphere.transform.position);
    }
    
    private void OnPostRender()
    {
        mat.SetPass(0);
        //mat.SetBuffer("value", cBuff);
        Graphics.DrawProcedural(MeshTopology.Points, particleSize*2, 1);
    }
    // Update is called once per frame
    void Update()
    {
        cShader.SetFloat("deltaT", Time.deltaTime);
        
        switch (mode)
        {
            case MODE.SQUARE:
                //for(int i=0;i<100;i++)
                    cShader.Dispatch(kernelHandle, particleSize / 8, 1, 1);
                break;
            case MODE.BUNNY:
                //for (int i = 0; i < 100; i++)
                    cShader.Dispatch(kernelHandle, particleSize / 8, 1, 1);
                break;
            case MODE.BUDDHA:
                //for (int i = 0; i < 100; i++)
                cShader.Dispatch(kernelHandle, particleSize / 8, 1, 1);
                break;
            case MODE.ARMA:
                //for (int i = 0; i < 100; i++)
                    cShader.Dispatch(kernelHandle, particleSize / 8, 1, 1);
                break;
            case MODE.DINO:
                //for (int i = 0; i < 100; i++)
                    cShader.Dispatch(kernelHandle, particleSize / 8, 1, 1);
                break;
            case MODE.A:
            case MODE.O:
                cShader.Dispatch(kernelHandle, particleSize / 8, 1, 1);
                break;
        }
    }
}

