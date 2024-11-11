using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class OnlyFiveWord : MonoBehaviour
{
    // ชื่อไฟล์ต้นทางและไฟล์ปลายทาง
    private string inputFilePath = "Assets/Resources/WordList.txt";  // ไฟล์ต้นทางที่มีคำทั้งหมด
    private string outputFilePath = "Assets/Resources/FiveLetterWords.txt"; // ไฟล์ปลายทางที่มีคำ 5 ตัวอักษร

    void Start()
    {
        FilterWordsWithFiveLetters();
    }

    void FilterWordsWithFiveLetters()
    {
        // โหลดคำจากไฟล์ต้นทาง
        string[] allWords = File.ReadAllLines(inputFilePath);

        // กรองให้เหลือเฉพาะคำที่มี 5 ตัวอักษร และเปลี่ยนเป็นพิมพ์ใหญ่ทั้งหมด
        List<string> fiveLetterWords = allWords
                                        .Select(word => word.Trim().ToUpper()) // ลบช่องว่างและทำให้เป็นพิมพ์ใหญ่
                                        .Where(word => word.Length == 5)       // เฉพาะคำที่มี 5 ตัวอักษร
                                        .Distinct()                            // กรองคำซ้ำ
                                        .ToList();

        // เขียนคำที่คัดกรองแล้วไปยังไฟล์ปลายทาง
        File.WriteAllLines(outputFilePath, fiveLetterWords);

        Debug.Log($"Filtered {fiveLetterWords.Count} five-letter words to {outputFilePath}");
    }
}
