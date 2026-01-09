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

        [Header("영수증 스크롤")]
        [Tooltip("영수증 패널의 ScrollRect (열릴 때 스크롤 위치 초기화용)")]
        [SerializeField] private ScrollRect receiptScrollRect;

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

            // 스크롤을 맨 아래로 초기화
            if (receiptScrollRect != null)
            {
                receiptScrollRect.verticalNormalizedPosition = 0f;
            }

            // 영수증 오디오 재생
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayReceiptAudio();
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

            // 메인화면 대기 음악 시작
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.ReturnToMain();
            }
        }
    }
}
