using System.Collections.Generic;

namespace Barcode
{
    /// <summary>
    /// 품목 정보를 담는 클래스
    /// </summary>
    public class ProductInfo
    {
        public ProductType Type { get; private set; }
        public string KoreanName { get; private set; }
        public PanelType PanelType { get; private set; }

        public ProductInfo(ProductType type, string koreanName, PanelType panelType)
        {
            Type = type;
            KoreanName = koreanName;
            PanelType = panelType;
        }
    }

    /// <summary>
    /// 모든 품목 데이터를 관리하는 데이터베이스
    /// </summary>
    public static class ProductDatabase
    {
        private static Dictionary<ProductType, ProductInfo> _products;

        static ProductDatabase()
        {
            InitializeProducts();
        }

        private static void InitializeProducts()
        {
            _products = new Dictionary<ProductType, ProductInfo>
            {
                // 개별 패널 품목 (14개) - 순서대로
                { ProductType.Banana, new ProductInfo(ProductType.Banana, "바나나", PanelType.Banana) },
                { ProductType.Lemon, new ProductInfo(ProductType.Lemon, "레몬", PanelType.Lemon) },
                { ProductType.Cabbage, new ProductInfo(ProductType.Cabbage, "배추", PanelType.Cabbage) },
                { ProductType.GreenOnion, new ProductInfo(ProductType.GreenOnion, "파", PanelType.GreenOnion) },
                { ProductType.Apple, new ProductInfo(ProductType.Apple, "국산 사과", PanelType.Apple) },
                { ProductType.AppleChile, new ProductInfo(ProductType.AppleChile, "칠레산 사과", PanelType.AppleChile) },
                { ProductType.Grape, new ProductInfo(ProductType.Grape, "국산 포도", PanelType.Grape) },
                { ProductType.GrapeUSA, new ProductInfo(ProductType.GrapeUSA, "미국산 포도", PanelType.GrapeUSA) },
                { ProductType.Potato, new ProductInfo(ProductType.Potato, "국산 감자", PanelType.Potato) },
                { ProductType.PotatoImport, new ProductInfo(ProductType.PotatoImport, "수입 감자", PanelType.PotatoImport) },
                { ProductType.Tangerine, new ProductInfo(ProductType.Tangerine, "국산 귤", PanelType.Tangerine) },
                { ProductType.TangerineImport, new ProductInfo(ProductType.TangerineImport, "수입 귤", PanelType.TangerineImport) },
                { ProductType.Orange, new ProductInfo(ProductType.Orange, "국산 오렌지", PanelType.Orange) },
                { ProductType.OrangeImport, new ProductInfo(ProductType.OrangeImport, "수입 오렌지", PanelType.OrangeImport) },

                // 공통 패널 품목 (11개) - 모두 PanelType.Common 사용
                { ProductType.Bread, new ProductInfo(ProductType.Bread, "빵", PanelType.Common) },
                { ProductType.Pineapple, new ProductInfo(ProductType.Pineapple, "파인애플", PanelType.Common) },
                { ProductType.IceCream, new ProductInfo(ProductType.IceCream, "아이스크림", PanelType.Common) },
                { ProductType.Drink, new ProductInfo(ProductType.Drink, "음료수", PanelType.Common) },
                { ProductType.Milk, new ProductInfo(ProductType.Milk, "우유", PanelType.Common) },
                { ProductType.Snack, new ProductInfo(ProductType.Snack, "과자", PanelType.Common) },
                { ProductType.Tissue, new ProductInfo(ProductType.Tissue, "휴지", PanelType.Common) },
                { ProductType.Detergent, new ProductInfo(ProductType.Detergent, "세제", PanelType.Common) },
                { ProductType.Shampoo, new ProductInfo(ProductType.Shampoo, "샴푸", PanelType.Common) },
                { ProductType.Hat, new ProductInfo(ProductType.Hat, "모자", PanelType.Common) },
                { ProductType.Gloves, new ProductInfo(ProductType.Gloves, "장갑", PanelType.Common) }
            };
        }

        /// <summary>
        /// 품목 타입으로 품목 정보 조회
        /// </summary>
        public static ProductInfo GetProductInfo(ProductType type)
        {
            if (_products.TryGetValue(type, out ProductInfo info))
            {
                return info;
            }
            return null;
        }

        /// <summary>
        /// 품목 타입에 해당하는 패널 타입 조회
        /// </summary>
        public static PanelType GetPanelType(ProductType type)
        {
            ProductInfo info = GetProductInfo(type);
            return info?.PanelType ?? PanelType.Common;
        }

        /// <summary>
        /// 품목의 한글 이름 조회
        /// </summary>
        public static string GetKoreanName(ProductType type)
        {
            ProductInfo info = GetProductInfo(type);
            return info?.KoreanName ?? "알 수 없음";
        }

        /// <summary>
        /// 바코드 문자열로 ProductType 조회
        /// 바코드에 "Banana", "Apple" 등의 텍스트가 들어있을 때 사용
        /// </summary>
        public static bool TryGetProductType(string barcode, out ProductType productType)
        {
            if (string.IsNullOrEmpty(barcode))
            {
                productType = default;
                return false;
            }

            // 공백 제거 및 정리
            string cleanBarcode = barcode.Trim();

            // enum 이름으로 직접 파싱 시도 (대소문자 무시)
            if (System.Enum.TryParse(cleanBarcode, true, out productType))
            {
                // 파싱된 값이 실제로 정의된 ProductType인지 확인
                if (_products.ContainsKey(productType))
                {
                    return true;
                }
            }

            productType = default;
            return false;
        }
    }
}
