using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UI;

namespace Barcode
{
    /// <summary>
    /// 바코드 스캔 시뮬레이터
    /// 스페이스바를 눌러 스캔, Inspector에서 품목 선택
    /// </summary>
    public class BarcodeScanner : MonoBehaviour
    {
        [Header("스캔 설정")]
        [Tooltip("스캔할 품목을 선택하세요")]
        [SerializeField] private ProductType currentProduct = ProductType.Banana;

        [Header("참조")]
        [Tooltip("PanelManager 컴포넌트를 할당하세요")]
        [SerializeField] private PanelManager panelManager;

        [Tooltip("ScanItemSpawner 컴포넌트를 할당하세요")]
        [SerializeField] private ScanItemSpawner scanItemSpawner;

        [Header("스캔 기록 (텍스트 방식 - 선택)")]
        [Tooltip("스캔 기록을 표시할 텍스트 (프리팹 방식 사용시 비워둬도 됨)")]
        [SerializeField] private TMP_Text scanHistoryText;

        [Header("디버그")]
        [SerializeField] private bool showDebugLog = true;

        private List<string> _scanHistory = new List<string>();

        private void Start()
        {
            ClearHistory();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Scan();
            }
        }

        /// <summary>
        /// 현재 설정된 품목을 스캔
        /// </summary>
        public void Scan()
        {
            if (panelManager == null)
            {
                Debug.LogError("[BarcodeScanner] PanelManager가 할당되지 않았습니다!");
                return;
            }

            // 품목 정보 조회
            string productName = ProductDatabase.GetKoreanName(currentProduct);
            PanelType panelType = ProductDatabase.GetPanelType(currentProduct);

            // 스캔 기록에 추가
            AddToHistory(productName);

            // 프리팹 아이템 생성
            if (scanItemSpawner != null)
            {
                scanItemSpawner.SpawnItem(currentProduct);
            }

            if (showDebugLog)
            {
                Debug.Log($"[BarcodeScanner] 스캔 완료: {productName} ({currentProduct}) → {panelType} 패널");
            }

            // 해당 패널 표시
            panelManager.ShowPanel(panelType);
        }

        /// <summary>
        /// 스캔 기록에 품목 추가
        /// </summary>
        private void AddToHistory(string productName)
        {
            _scanHistory.Add(productName);
            UpdateHistoryText();
        }

        /// <summary>
        /// 스캔 기록 텍스트 업데이트
        /// </summary>
        private void UpdateHistoryText()
        {
            if (scanHistoryText != null)
            {
                scanHistoryText.text = string.Join(", ", _scanHistory);
            }
        }

        /// <summary>
        /// 스캔 기록 초기화
        /// </summary>
        public void ClearHistory()
        {
            _scanHistory.Clear();
            UpdateHistoryText();

            // 생성된 프리팹 아이템도 삭제
            if (scanItemSpawner != null)
            {
                scanItemSpawner.ClearAllItems();
            }
        }

        /// <summary>
        /// 스캔 기록 목록 반환
        /// </summary>
        public List<string> GetHistory()
        {
            return new List<string>(_scanHistory);
        }

        /// <summary>
        /// 스캔할 품목 변경 (코드에서 호출용)
        /// </summary>
        public void SetProduct(ProductType product)
        {
            currentProduct = product;
            if (showDebugLog)
            {
                Debug.Log($"[BarcodeScanner] 품목 변경: {ProductDatabase.GetKoreanName(product)}");
            }
        }

        /// <summary>
        /// 현재 설정된 품목 조회
        /// </summary>
        public ProductType GetCurrentProduct()
        {
            return currentProduct;
        }
    }
}
