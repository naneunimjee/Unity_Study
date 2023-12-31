using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DirectoryController : MonoBehaviour
{
    [SerializeField]
    private FileLoaderSystem fileLoaderSystem; //확장자별 파일 처리 시스템 (Load And Play)

    private DirectoryInfo defaultDirectory; //기본 폴더 (바탕화면)
    private DirectoryInfo currentDirectory;
    private DirectorySpawner directorySpawner;

    private void Awake()
    {
        //프로그램이 최상단에 활성화 상태가 아니어도 플레이
        Application.runInBackground = true;

        directorySpawner = GetComponent<DirectorySpawner>();
        directorySpawner.Setup(this);

        //최초경로를 바탕화면으로 설정
        //Environment.GetFolerPath() : 윈도우에 존재하는 폴더 경로를 얻어옴
        //Environment.SpecialFolder : 윈도우에 존재하는 특수 폴더 열거형
        string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        currentDirectory = new DirectoryInfo(desktopFolder);
        defaultDirectory = new DirectoryInfo(desktopFolder);

        //현재 폴더에 있는 디렉토리, 파일을 생성
        UpdateDirectory(currentDirectory);
    }

    private void Update()
    {
        //ESC 키를 눌렀을 때 바탕화면 폴더로 이동
        if ( Input.GetKeyDown(KeyCode.Escape))
        {
            UpdateDirectory(defaultDirectory);
        }

        //백스페이스 키를 눌렀을 때 상위 폴더로 이동
        if ( Input.GetKeyDown(KeyCode.Backspace))
        {
            MoveToParentFolder(currentDirectory)
;        }
    }
    //현재 폴더 정보를 업데이트 하는 업데이트 함수 설정
    private void UpdateDirectory(DirectoryInfo directory)
    {

        //현재 경로 설정
        currentDirectory = directory;

        //현재 폴더에 존재하는 모든 폴더, 파일 PanelData를 생성
        directorySpawner.UpdateDirectory(currentDirectory);
    }

    public void UpdateInputs(string data)
    {

        //1. 선택한 목록(data)이 "..."이면 상위 폴더 이동
        if (data.Equals("..."))
        {
            MoveToParentFolder(currentDirectory);
            return;
        }

        //2. 선택한 목록(data)이 폴더이면 선택한 폴더 내부로 이동

        foreach (DirectoryInfo directory in currentDirectory.GetDirectories())
        {
            if (data.Equals(directory.Name))
            {
                UpdateDirectory(directory);
                return;
            }
        }
        //3. 선택한 목록(data)가 파일이면 확장자에 따라 처리
        foreach (FileInfo file in currentDirectory.GetFiles())
        {
            if (data.Equals(file.Name))
            {
                //Debug.Log($"선택한 파일의 이름 : {file.FullName}")l

                fileLoaderSystem.LoadFile(file);
            }
        }
    }    
    
    //상위 폴더로 이동
    private void MoveToParentFolder(DirectoryInfo directory)
    {
        if (directory.Parent == null)
        {return;}

        UpdateDirectory(directory.Parent);
    }
}


