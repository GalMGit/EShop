export type ProductListItemResponse = {
    id: string;
    name: string;
    thumbnailUrl?: string;
    price: number;
    categoryId: string;
    brandId: string;
    createdAt: string;
    availableQuantity: number;
}