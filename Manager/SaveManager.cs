using System;
using System.IO;
using UnityEngine;

public enum ELoadResult
{
    Success, // 정상
    NotFound, // 파일 없음, 새 게임
    Recovered, // 본 파일 손상, 백업으로 복구
    Corrupted, // 본 파일 손상, 백업 실패
}

public static class SaveManager
{
    const string FileName = "save.json";

    static string _savePath;
    static string SavePath
    {
        get
        {
            if (string.IsNullOrEmpty(_savePath))
            {
                _savePath = Path.Combine(Application.persistentDataPath, FileName);
            }
            return _savePath;
        }
    }

    static string TempPath => SavePath + ".tmp";
    static string BackupPath => SavePath + ".bak";

    static bool TrySave(SaveData data)
    {
        if (data == null) { return false; }

        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(TempPath, json); // 임시 파일에 먼저 기록

            if (File.Exists(SavePath)) // 기존 파일은 .bak으로 밀림
            {
                File.Replace(TempPath, SavePath, BackupPath);
            }
            else
            {
                File.Move(TempPath, SavePath);
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveManager] 저장 실패: {e}");
            DeleteTemp();
            return false;
        }
    }

    static void DeleteTemp()
    {
        if (File.Exists(TempPath))
        {
            File.Delete(TempPath);
        }
    }

    static ELoadResult Load(out SaveData data)
    {
        bool exists = File.Exists(SavePath);

        if (TryRead(SavePath, out data)) // 파일 읽기 성공
        {
            return ELoadResult.Success;
        }

        if (TryRead(BackupPath, out data)) // 백업 파일 읽기 성공
        {
            Debug.LogWarning("[SaveManager] 본 파일 손상. 백업으로 복구했습니다.");
            return ELoadResult.Recovered;
        }

        if (exists) // 파일이 있으나 손상됨
        {
            Debug.LogWarning("[SaveManager] 본 파일과 백업본 모두 불러올 수 없습니다. 새 파일을 불러옵니다.");
            return ELoadResult.Corrupted;
        }

        return ELoadResult.NotFound; // 파일 없음
    }

    static void CreateDefaultSave(out SaveData data)
    {
        InventoryData inventory = new InventoryData();
        PlayerWalletData playerStat = new PlayerWalletData();
        SettingData settingData = new SettingData();
        settingData.BgmVolume = 1f;
        settingData.SfxVolume = 1f;
        settingData.MouseSensitivity = 20f;
        data = new SaveData(inventory, playerStat, settingData);
    }

    // 경로에서 읽은 후 data에 쓰기
    static bool TryRead(string path, out SaveData data)
    {
        data = null;
        if (File.Exists(path) == false) { return false; } // 파일이 없음

        try
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<SaveData>(json);
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveManager] 읽기 실패 ({Path.GetFileName(path)}): {e.Message}");
            data = null;
            return false;
        }

        // 파싱은 되지만 빈 JSON이 오면 손상으로 처리
        if (data == null || data.Inventory == null || data.Wallet == null || data.Setting == null)
        {
            data = null;
            return false;
        }

        if (data.Version > SaveData.CurrentVersion) // 구버전 빌드로 신버전을 열었을 때
        {
            Debug.LogError($"[SaveManager] 지원하지 않는 세이브 버전 {data.Version} (현재 {SaveData.CurrentVersion})");
            data = null;
            return false;
        }

        // 마이그래이션 코드 작성

        return true;
    }

    /// <summary> 게임 저장 </summary>
    public static void SaveGame(IInvenSave invenSave, IPlayerWalletSave walletSave, ISettingSave setting)
    {
        InventoryData invenData = invenSave.GetSaveData();
        PlayerWalletData walletData = walletSave.GetSaveData();
        SettingData settingData = setting.GetSaveData();

        SaveData data = new SaveData(invenData, walletData, settingData);

        TrySave(data); // 저장
    }

    /// <summary> 게임 불러오기 </summary>
    public static void LoadGame(IInvenSave invenSave, IPlayerWalletSave walletSave, ISettingSave setting)
    {
        ELoadResult result = Load(out SaveData data); // 불러오기

        // 파일이 없거나 손상됐으면 새로 만듦
        if (result == ELoadResult.NotFound || result == ELoadResult.Corrupted)
        {
            CreateDefaultSave(out data);
        }

        invenSave.ApplySaveData(data.Inventory);
        walletSave.ApplySaveData(data.Wallet);
        setting.ApplySaveData(data.Setting);
    }

    /// <summary> 게임 데이터 초기화 </summary>
    public static void ResetGame()
    {
        if (File.Exists(SavePath)) // 원본 삭제
        {
            File.Delete(SavePath);
        }

        if (File.Exists(BackupPath)) // 백업본 삭제
        {
            File.Delete(BackupPath);
        }
    }
}
