using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    CharacterController control;

    float speedRun = 5;
    float speedJump = 8;
    float gravity = 16;

    Vector3 moveDir;
    float accel = 1;
    bool canMove = true;

    float mouseSpeed = 90;
    private Vector3 rot;

    bool isEsc;

    public bool InfectedPlayer;  //게임 시작 시 플레이어가 일반인인지 감염자인지 결정
    public bool OnInfected;                //감염자 모드 실행
    public lightMapManager Night;              // 밤

    Transform voterPos;
    Transform executionPos;
    public GameObject originPos;

    bool BeAttacked = false;

    // Use this for initialization
    void Awake()
    {
        control = GetComponent<CharacterController>();
        
    }

    private void Start()
    {
        Cursor.visible = false;    //마우스커서 숨기기
        Cursor.lockState = CursorLockMode.Locked;
        Night = GameObject.Find("LightMapManager").GetComponent<lightMapManager>();
        voterPos = GameObject.Find("voterPosition").transform;
        executionPos = GameObject.Find("executionPosition").transform;
        originPos = Resources.Load("originPostion") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        ESCMenu();
        MovePlayer();
        InfectedModState();
        GotoVote();
        if (isEsc)
            return;
        InputMouse();
        InputKeys();
    }

    //입력
    private void InputKeys()
    {
        moveDir.z = Input.GetAxis("Vertical");
        moveDir.x = Input.GetAxis("Horizontal");
        //점프
        bool bJump = control.isGrounded && Input.GetButtonDown("Jump"); /*|| Input.GetButtonDown("OVRJump")*/   //isGrounded 땅에 있는지 검출
        if (bJump) moveDir.y = speedJump * accel;

        if (InfectedPlayer)  //감염자 플레이어가
        {
            if (Night.night) //밤에
            {
                if (!OnInfected && Input.GetKeyDown(KeyCode.LeftShift)) //변신 상태가 아닐때 쉬프트를 누르면
                {
                    OnInfected = !OnInfected;           //변신모드
                }
                else if (OnInfected && Input.GetKeyDown(KeyCode.LeftShift))
                {
                    OnInfected = !OnInfected;
                }
            }
            else
            {
                OnInfected = false;
            }
        }

    }

    //TEST asdfdsaf

    private void InputMouse()
    {
        //상하 회전
        rot.x -= Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;
        rot.x = Mathf.Clamp(rot.x, -90, 90);
        Camera.main.transform.localRotation = Quaternion.Euler(rot.x, 0f, 0f);

        //좌우 회전
        rot.y = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        transform.Rotate(0f, rot.y, 0f);
    }

    //이동
    private void MovePlayer()
    {
        moveDir.z = (canMove ? moveDir.z : 0) * speedRun * accel;
        moveDir.x = (canMove ? moveDir.x : 0) * speedRun * accel;


        //중력 중요**
        moveDir.y -= gravity * Time.deltaTime;
        if (control.isGrounded && moveDir.y < -1)
        {
            moveDir.y = -1;
        }
        Vector3 forword = transform.TransformDirection(moveDir);
        control.Move(forword * Time.deltaTime);
    }

    public void ESCMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isEsc)
            {
                isEsc = !isEsc;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                moveDir.z = 0;
                moveDir.x = 0;
            }
            else
            {
                isEsc = !isEsc;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

    }

    void InfectedModState() //감염자로 되었을 때 능력치
    {
        if (OnInfected) { accel = 1.5f; }
        else accel = 1f;
    }

    private void _OnControllerColliderHit(ControllerColliderHit hit) //캐릭터 컨트롤러 충돌 판정 함수
    {

    }

    private void OnTriggerEnter(Collider other)
    {
    }

    void GotoVote() //테스트 코드
    {
        if (Input.GetButtonDown("Fire1"))
        {
            originPos.transform.position = this.transform.position;
            this.transform.position = voterPos.transform.position;
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            this.transform.position = originPos.transform.position;
            //this.transform.position = voterPos.transform.position;
        }
    }

    void GoToExecutor()
    {
        originPos.transform.position = this.transform.position;
        if (!Night && BeAttacked)
        {  
            this.transform.position = executionPos.transform.position;
        }
        else
        {
            this.transform.position = voterPos.transform.position;
        }
        
    }
    void ExitFromExecutor()
    {
        this.transform.position = originPos.transform.position;
    }
}

//TEST create by kmg