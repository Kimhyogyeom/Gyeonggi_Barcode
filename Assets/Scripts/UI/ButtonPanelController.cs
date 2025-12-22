using UnityEngine;
using UnityEngine.UI;
using Barcode;

namespace UI
{
    /// <summary>
    /// 버튼들을 관리하는 컨트롤러
    /// </summary>
    public class ButtonPanelController : MonoBehaviour
    {
        [Header("참조")]
        [SerializeField] private PanelManager panelManager;
        [SerializeField] private BarcodeScanner barcodeScanner;

        [Header("영수증보기 버튼 (각 패널에 있는 버튼들)")]
        [SerializeField] private Button[] receiptButtons;

        [Header("Exit 버튼 (각 패널에 있는 버튼들)")]
        [SerializeField] private Button[] exitButtons;

        [Header("패널 설정")]
        [SerializeField] private GameObject receiptPanel;
        [SerializeField] private GameObject mainPanel;

        [Header("비활성화할 패널들")]
        [SerializeField] private GameObject[] panelsToHide;

        private void Start()
        {
            SetupButtons();
        }

        private void SetupButtons()
        {
            // 영수증보기 버튼 설정
            foreach (var btn in receiptButtons)
            {
                if (btn != null)
                {
                    btn.onClick.AddListener(OpenReceiptPanel);
                }
            }

            // Exit 버튼 설정
            foreach (var btn in exitButtons)
            {
                if (btn != null)
                {
                    btn.onClick.AddListener(ExitToMain);
                }
            }
        }

        /// <summary>
        /// 영수증 패널 열기
        /// </summary>
        public void OpenReceiptPanel()
        {
            if (panelManager != null)
            {
                panelManager.HideAllPanels();
            }

            if (receiptPanel != null)
            {
                receiptPanel.SetActive(true);
            }
        }

        /// <summary>
        /// Exit: 메인 화면으로 돌아가기
        /// </summary>
        public void ExitToMain()
        {
            // 스캔 기록 초기화
            if (barcodeScanner != null)
            {
                barcodeScanner.ClearHistory();
            }

            // 모든 패널 숨기기
            if (panelManager != null)
            {
                panelManager.HideAllPanels();
            }

            // 추가로 숨길 패널들 비활성화
            foreach (var panel in panelsToHide)
            {
                if (panel != null)
                {
                    panel.SetActive(false);
                }
            }

            // 메인 화면 활성화
            if (mainPanel != null)
            {
                mainPanel.SetActive(true);
            }
        }
    }
}
