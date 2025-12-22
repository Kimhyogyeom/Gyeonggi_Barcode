namespace Barcode
{
    /// <summary>
    /// 스캔 가능한 모든 품목 종류 (총 25개)
    /// </summary>
    public enum ProductType
    {
        // 개별 패널 품목 (14개) - 순서대로
        Banana,             // 바나나
        Lemon,              // 레몬
        Cabbage,            // 배추
        GreenOnion,         // 파
        Apple,              // 사과
        AppleChile,         // 칠레산 사과
        Grape,              // 포도
        GrapeUSA,           // 미국산 포도
        Potato,             // 감자
        PotatoImport,       // 수입 감자
        Tangerine,          // 귤
        TangerineImport,    // 수입 귤
        Orange,             // 오렌지
        OrangeImport,       // 수입 오렌지

        // 공통 패널 품목 (11개) - PanelWindowReady 사용
        Bread,      // 빵
        Pineapple,  // 파인애플
        IceCream,   // 아이스크림
        Drink,      // 음료수
        Milk,       // 우유
        Snack,      // 과자
        Tissue,     // 휴지
        Detergent,  // 세제
        Shampoo,    // 샴푸
        Hat,        // 모자
        Gloves      // 장갑
    }

    /// <summary>
    /// 패널 타입 (총 15개)
    /// </summary>
    public enum PanelType
    {
        Banana,             // PanelWindowBanana
        Lemon,              // PanelWindowLemon
        Cabbage,            // PanelWindowBeachoo
        GreenOnion,         // PanelWindowPa
        Apple,              // PanelWindowApple
        AppleChile,         // PanelWindowAppleChile (칠레산 사과)
        Grape,              // PanelWindowPodo
        GrapeUSA,           // PanelWindowGrapeUSA (미국산 포도)
        Potato,             // PanelWindowGamja
        PotatoImport,       // PanelWindowPotatoImport (수입 감자)
        Tangerine,          // PanelWindowGul
        TangerineImport,    // PanelWindowTangerineImport (수입 귤)
        Orange,             // PanelWindowOrange
        OrangeImport,       // PanelWindowOrangeImport (수입 오렌지)
        Common              // PanelWindowReady (11개 품목 공통)
    }
}
