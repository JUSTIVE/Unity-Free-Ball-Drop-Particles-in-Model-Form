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
    };
    public Camera cam;
    public enum MODE { SQUARE, BUNNY, REFERENCE,ARMA, DINO};
    public MODE mode;
    public Text text;
    public Shader rainbowShader;
    public Shader whiteShader;
    data[] value;
    private ComputeBuffer cBuff = null;
    public ComputeShader cShader;
    private Material mat;
    private int kernelHandle;
    private int width, height, depth;
    private int particleSize;
    private int masterCount;
    private double dts;
    public int index;
    public bool isText = false;
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
        width = mapper[index];
        height = mapper[index];
        depth = 1;
        particleSize = width * height * depth;
        value = new data[particleSize];
        for (int i = 0; i < particleSize; i++)
        {
            value[i].pos = new Vector3(0, 0, 0);
            value[i].vel = new Vector3(0, 0, 0);
        }
        float dx = 1.0f / (2.0f * width), dy = 1.0f / (1.0f * height), dz = 1.0f / depth;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < depth; k++)
                {
                    value[i * height + j].pos.x = (dx * i) - 0.25f;
                    value[i * height + j].pos.y = dy * j + Random.Range(0, 0);
                    value[i * height + j].pos.z = k * dz;
                }
            }
        }
        Debug.Log(value[0].pos.x+" "+value[0].pos.y+" "+value[0].pos.z);
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
            gb.transform.position.Set(value[i].pos.x,
                value[i].pos.y,
                value[i].pos.z);
            gb.transform.Rotate(new Vector3(1, 1, 0), 45);

            //value[i].pos.x = gb.transform.position.x;
            //value[i].pos.y = gb.transform.position.x;
            //value[i].pos.z = gb.transform.position.x;
            value[i].pos = Quaternion.Euler(225.0f, 200.0f, 0.0f) * value[i].pos;
        }
        //Debug.Log(gb.transform.position.x);
        Debug.Log(value[0].pos.x + " " + value[0].pos.y + " " + value[0].pos.z);
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
            value[i].pos.y = float.Parse(tempArr[2]) / scaler-1.2f;
            value[i].pos.z = float.Parse(tempArr[3]) / scaler;
            //gb.transform.position.Set(value[i].pos.x,
            //    value[i].pos.y,
            //    value[i].pos.z);
            //gb.transform.Rotate(new Vector3(1, 1, 0), 45);
            //value[i].pos = Quaternion.Euler(225.0f, 200.0f, 0.0f) * value[i].pos;
        }
        //Debug.Log(gb.transform.position.x);
        
        Debug.Log(value[0].pos.x + " " + value[0].pos.y + " " + value[0].pos.z);
    } 
    void initArma()
    {
        StreamReader sr = new StreamReader("Assets/arma.txt");
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
            gb.transform.position.Set(value[i].pos.x,
                value[i].pos.y,
                value[i].pos.z);
            gb.transform.Rotate(new Vector3(1, 1, 0), 45);

            //value[i].pos.x = gb.transform.position.x;
            //value[i].pos.y = gb.transform.position.x;
            //value[i].pos.z = gb.transform.position.x;
            value[i].pos = Quaternion.Euler(225.0f, 200.0f, 0.0f) * value[i].pos;
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
        if (isText)
            text.text = mapper[index] + "\n";
        //frame
        dts = 0;
        masterCount = 0;
        //other
        switch (mode)
        {
            case MODE.SQUARE:
                mat = new Material(rainbowShader);
                initSquare();
                break;
            case MODE.BUNNY:
                mat = new Material(rainbowShader);
                scaler = 20.0f;
                initBunny();
                break;
            case MODE.REFERENCE:
                break;
            case MODE.ARMA:
                mat = new Material(rainbowShader);
                scaler = 1.0f;
                initArma();
                break;
            case MODE.DINO:
                mat = new Material(rainbowShader);
                scaler = 50.0f;
                initDino();
                break;
        }
        if (cBuff != null)
        {
            cBuff.Release();
        }
        
        Debug.Log("particleSize = "+particleSize);
        cBuff = new ComputeBuffer(particleSize, 24);
        mat.SetFloat("x", (float)width);
        mat.SetFloat("y", (float)height);
        mat.SetFloat("z", (float)width);

        kernelHandle = cShader.FindKernel("CSMain");
        cBuff.SetData(value);
        mat.SetBuffer("value", cBuff);
        cShader.SetBuffer(kernelHandle, "value", cBuff);

    }
    
    private void OnPostRender()
    {
        mat.SetPass(0);
        //mat.SetBuffer("value", cBuff);
        Graphics.DrawProcedural(MeshTopology.Points, particleSize, 1);
    }
    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case MODE.SQUARE:
                cShader.Dispatch(kernelHandle, particleSize / 6, 1, 1);
                break;
            case MODE.BUNNY:
                cShader.Dispatch(kernelHandle, particleSize / 6, 1, 1);
                break;
            case MODE.ARMA:
                cShader.Dispatch(kernelHandle, particleSize / 6, 1, 1);
                break;
            case MODE.DINO:
                cShader.Dispatch(kernelHandle, particleSize / 6, 1, 1);
                break;
        }
        
        //for frame
        masterCount++;
        dts += (1 / Time.deltaTime);
        if (masterCount <= 2001)
            if (masterCount % 200 == 0)
            {
                if (isText)
                    text.text += dts / 200.0 + "\n";
                dts = 0;
            }
        //if (Input.GetMouseButtonDown(0))
        //{
        //    index = index > 5 ? 0 : index + 1;
        //    init();
        //}
        //cBuff.GetData(value);
        //GameObject gb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //gb.transform.position.Set(value[0].pos.x, value[0].pos.y, value[0].pos.z);
        //gb.transform.lossyScale.Set(0.01f, 0.01f, 0.01f);
        //gb = null;
        //Debug.Log(value[0].pos.x + " " + value[0].pos.y + " " + value[0].pos.z);
        //Debug.Log()
    }
}
