using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UI;

namespace Barcode
{
    /// <summary>
    /// 바코드 스캐너
    /// USB 바코드 스캐너 입력을 받아 처리 (키보드처럼 동작)
    /// </summary>
    public class BarcodeScanner : MonoBehaviour
    {
        [Header("스캔 설정")]
        [Tooltip("시뮬레이션 모드: 스페이스바로 테스트할 때 사용할 품목")]
        [SerializeField] private ProductType simulationProduct = ProductType.Banana;

        [Tooltip("시뮬레이션 모드 사용 (체크 해제시 실제 바코드 스캐너만 동작)")]
        [SerializeField] private bool useSimulationMode = false;

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

        // 바코드 입력 버퍼 (USB 스캐너가 키보드처럼 문자를 입력함)
        private string _barcodeBuffer = "";

        private void Start()
        {
            ClearHistory();
        }

        private void Update()
        {
            // 시뮬레이션 모드: 스페이스바로 테스트
            if (useSimulationMode && Input.GetKeyDown(KeyCode.Space))
            {
                ScanProduct(simulationProduct);
                return;
            }

            // 실제 바코드 스캐너 입력 처리
            ProcessBarcodeInput();
        }

        /// <summary>
        /// USB 바코드 스캐너 입력 처리
        /// 스캐너는 키보드처럼 문자를 입력하고 마지막에 Enter를 보냄
        /// </summary>
        private void ProcessBarcodeInput()
        {
            // 이번 프레임에 입력된 문자 확인
            string inputString = Input.inputString;

            if (string.IsNullOrEmpty(inputString))
                return;

            foreach (char c in inputString)
            {
                // Enter 키 (줄바꿈) = 스캔 완료
                if (c == '\n' || c == '\r')
                {
                    if (!string.IsNullOrEmpty(_barcodeBuffer))
                    {
                        ProcessScannedBarcode(_barcodeBuffer);
                        _barcodeBuffer = "";
                    }
                }
                // 일반 문자 = 버퍼에 추가
                else if (!char.IsControl(c))
                {
                    _barcodeBuffer += c;
                }
            }
        }

        /// <summary>
        /// 스캔된 바코드 문자열 처리
        /// </summary>
        private void ProcessScannedBarcode(string barcode)
        {
            if (showDebugLog)
            {
                Debug.Log($"[BarcodeScanner] 바코드 입력: \"{barcode}\"");
            }

            // 바코드 문자열로 ProductType 찾기
            if (ProductDatabase.TryGetProductType(barcode, out ProductType productType))
            {
                ScanProduct(productType);
            }
            else
            {
                Debug.LogWarning($"[BarcodeScanner] 알 수 없는 바코드: \"{barcode}\"");
            }
        }

        /// <summary>
        /// 지정된 품목을 스캔 처리
        /// </summary>
        public void ScanProduct(ProductType product)
        {
            if (panelManager == null)
            {
                Debug.LogError("[BarcodeScanner] PanelManager가 할당되지 않았습니다!");
                return;
            }

            // 품목 정보 조회
            string productName = ProductDatabase.GetKoreanName(product);
            PanelType panelType = ProductDatabase.GetPanelType(product);

            // 스캔 기록에 추가
            AddToHistory(productName);

            // 프리팹 아이템 생성
            if (scanItemSpawner != null)
            {
                scanItemSpawner.SpawnItem(product);
            }

            if (showDebugLog)
            {
                Debug.Log($"[BarcodeScanner] 스캔 완료: {productName} ({product}) → {panelType} 패널");
            }

            // 해당 패널 표시
            panelManager.ShowPanel(panelType);
        }

        /// <summary>
        /// 시뮬레이션 품목을 스캔 (하위 호환성)
        /// </summary>
        public void Scan()
        {
            ScanProduct(simulationProduct);
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
        /// 시뮬레이션 품목 변경 (코드에서 호출용)
        /// </summary>
        public void SetSimulationProduct(ProductType product)
        {
            simulationProduct = product;
            if (showDebugLog)
            {
                Debug.Log($"[BarcodeScanner] 시뮬레이션 품목 변경: {ProductDatabase.GetKoreanName(product)}");
            }
        }

        /// <summary>
        /// 현재 시뮬레이션 품목 조회
        /// </summary>
        public ProductType GetSimulationProduct()
        {
            return simulationProduct;
        }

        /// <summary>
        /// 시뮬레이션 모드 토글
        /// </summary>
        public void SetSimulationMode(bool enabled)
        {
            useSimulationMode = enabled;
            if (showDebugLog)
            {
                Debug.Log($"[BarcodeScanner] 시뮬레이션 모드: {(enabled ? "켜짐" : "꺼짐")}");
            }
        }

        /// <summary>
        /// 현재 바코드 버퍼 초기화 (필요시)
        /// </summary>
        public void ClearBarcodeBuffer()
        {
            _barcodeBuffer = "";
        }
    }
}
