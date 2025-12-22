using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Barcode;

namespace UI
{
    /// <summary>
    /// 품목별 스프라이트 매핑
    /// </summary>
    [Serializable]
    public class ProductSpriteMapping
    {
        public ProductType productType;
        public Sprite sprite;
    }

    /// <summary>
    /// 스캔한 품목을 프리팹으로 생성하여 리스트에 추가
    /// </summary>
    public class ScanItemSpawner : MonoBehaviour
    {
        [Header("프리팹 설정")]
        [Tooltip("생성할 프리팹 (첫번째 자식: TMP_Text, 두번째 자식: Image)")]
        [SerializeField] private GameObject itemPrefab;

        [Header("생성 위치")]
        [Tooltip("프리팹이 생성될 부모 (Grid Layout Group)")]
        [SerializeField] private Transform parentGrid;

        [Header("품목별 스프라이트")]
        [SerializeField] private ProductSpriteMapping[] productSprites;

        /// <summary>
        /// 스캔한 품목 아이템 생성
        /// </summary>
        public void SpawnItem(ProductType productType)
        {
            if (itemPrefab == null)
            {
                Debug.LogError("[ScanItemSpawner] itemPrefab이 할당되지 않았습니다!");
                return;
            }

            if (parentGrid == null)
            {
                Debug.LogError("[ScanItemSpawner] parentGrid가 할당되지 않았습니다!");
                return;
            }

            // 프리팹 생성
            GameObject newItem = Instantiate(itemPrefab, parentGrid);

            // 첫번째 자식: TMP_Text - 품명 설정
            if (newItem.transform.childCount > 0)
            {
                TMP_Text textComponent = newItem.transform.GetChild(0).GetComponent<TMP_Text>();
                if (textComponent != null)
                {
                    textComponent.text = ProductDatabase.GetKoreanName(productType);
                }
            }

            // 두번째 자식: Image - 스프라이트 설정
            if (newItem.transform.childCount > 1)
            {
                Image imageComponent = newItem.transform.GetChild(1).GetComponent<Image>();
                if (imageComponent != null)
                {
                    Sprite sprite = GetSpriteForProduct(productType);
                    if (sprite != null)
                    {
                        imageComponent.sprite = sprite;
                    }
                }
            }

            Debug.Log($"[ScanItemSpawner] {ProductDatabase.GetKoreanName(productType)} 아이템 생성");
        }

        /// <summary>
        /// 품목에 해당하는 스프라이트 찾기
        /// </summary>
        private Sprite GetSpriteForProduct(ProductType productType)
        {
            foreach (var mapping in productSprites)
            {
                if (mapping.productType == productType)
                {
                    return mapping.sprite;
                }
            }
            return null;
        }

        /// <summary>
        /// 모든 생성된 아이템 삭제
        /// </summary>
        public void ClearAllItems()
        {
            if (parentGrid == null) return;

            for (int i = parentGrid.childCount - 1; i >= 0; i--)
            {
                Destroy(parentGrid.GetChild(i).gameObject);
            }

            Debug.Log("[ScanItemSpawner] 모든 아이템 삭제");
        }
    }
}
