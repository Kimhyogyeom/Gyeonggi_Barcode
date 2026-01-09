using UnityEngine;
using UnityEngine.UI;
using Barcode;

namespace UI
{
    /// <summary>
    /// 패널 표시/숨김을 관리하는 매니저
    /// Inspector에서 각 패널 GameObject를 할당해야 함
    /// </summary>
    public class PanelManager : MonoBehaviour
    {
        [Header("개별 패널 (14개) - 순서대로")]
        [SerializeField] private GameObject panelBanana;           // PanelWindowBanana
        [SerializeField] private GameObject panelLemon;            // PanelWindowLemon
        [SerializeField] private GameObject panelCabbage;          // PanelWindowBeachoo
        [SerializeField] private GameObject panelGreenOnion;       // PanelWindowPa
        [SerializeField] private GameObject panelApple;            // PanelWindowApple
        [SerializeField] private GameObject panelAppleChile;       // 칠레산 사과
        [SerializeField] private GameObject panelGrape;            // PanelWindowPodo
        [SerializeField] private GameObject panelGrapeUSA;         // 미국산 포도
        [SerializeField] private GameObject panelPotato;           // PanelWindowGamja
        [SerializeField] private GameObject panelPotatoImport;     // 수입 감자
        [SerializeField] private GameObject panelTangerine;        // PanelWindowGul
        [SerializeField] private GameObject panelTangerineImport;  // 수입 귤
        [SerializeField] private GameObject panelOrange;           // PanelWindowOrange
        [SerializeField] private GameObject panelOrangeImport;     // 수입 오렌지

        [Header("공통 패널 (11개 품목용)")]
        [SerializeField] private GameObject panelCommon;           // PanelWindowReady

        [Header("영수증 스크롤 (Common 패널용)")]
        [Tooltip("Common 패널의 ScrollRect (표시될 때 스크롤 위치 초기화용)")]
        [SerializeField] private ScrollRect commonScrollRect;

        private GameObject _currentActivePanel;

        private void Start()
        {
            HideAllPanels();
        }

        /// <summary>
        /// 지정된 패널 타입의 패널을 표시
        /// </summary>
        public void ShowPanel(PanelType panelType)
        {
            HideAllPanels();

            GameObject targetPanel = GetPanelByType(panelType);
            if (targetPanel != null)
            {
                targetPanel.SetActive(true);
                _currentActivePanel = targetPanel;
                Debug.Log($"[PanelManager] {panelType} 패널 표시");

                // Common 패널(영수증)일 경우 스크롤 초기화 + 영수증 오디오 재생
                if (panelType == PanelType.Common)
                {
                    if (commonScrollRect != null)
                    {
                        commonScrollRect.verticalNormalizedPosition = 0f;
                    }

                    // 영수증 오디오 재생 (푸드마일리지)
                    if (AudioManager.Instance != null)
                    {
                        AudioManager.Instance.PlayReceiptAudio();
                    }
                }
            }
            else
            {
                Debug.LogWarning($"[PanelManager] {panelType} 패널을 찾을 수 없습니다. Inspector에서 할당해주세요.");
            }
        }

        /// <summary>
        /// 모든 패널 숨기기
        /// </summary>
        public void HideAllPanels()
        {
            SetPanelActive(panelBanana, false);
            SetPanelActive(panelLemon, false);
            SetPanelActive(panelCabbage, false);
            SetPanelActive(panelGreenOnion, false);
            SetPanelActive(panelApple, false);
            SetPanelActive(panelAppleChile, false);
            SetPanelActive(panelGrape, false);
            SetPanelActive(panelGrapeUSA, false);
            SetPanelActive(panelPotato, false);
            SetPanelActive(panelPotatoImport, false);
            SetPanelActive(panelTangerine, false);
            SetPanelActive(panelTangerineImport, false);
            SetPanelActive(panelOrange, false);
            SetPanelActive(panelOrangeImport, false);
            SetPanelActive(panelCommon, false);

            _currentActivePanel = null;
        }

        /// <summary>
        /// 현재 활성화된 패널 숨기기
        /// </summary>
        public void HideCurrentPanel()
        {
            if (_currentActivePanel != null)
            {
                _currentActivePanel.SetActive(false);
                _currentActivePanel = null;
            }
        }

        /// <summary>
        /// 현재 패널이 활성화되어 있는지 확인
        /// </summary>
        public bool IsPanelActive()
        {
            return _currentActivePanel != null && _currentActivePanel.activeSelf;
        }

        private GameObject GetPanelByType(PanelType panelType)
        {
            return panelType switch
            {
                PanelType.Banana => panelBanana,
                PanelType.Lemon => panelLemon,
                PanelType.Cabbage => panelCabbage,
                PanelType.GreenOnion => panelGreenOnion,
                PanelType.Apple => panelApple,
                PanelType.AppleChile => panelAppleChile,
                PanelType.Grape => panelGrape,
                PanelType.GrapeUSA => panelGrapeUSA,
                PanelType.Potato => panelPotato,
                PanelType.PotatoImport => panelPotatoImport,
                PanelType.Tangerine => panelTangerine,
                PanelType.TangerineImport => panelTangerineImport,
                PanelType.Orange => panelOrange,
                PanelType.OrangeImport => panelOrangeImport,
                PanelType.Common => panelCommon,
                _ => null
            };
        }

        private void SetPanelActive(GameObject panel, bool active)
        {
            if (panel != null)
            {
                panel.SetActive(active);
            }
        }
    }
}
