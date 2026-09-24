export type ProductDetailsResponse = {
    id: string;
    name: string;
    brand: string;
    description: string;
    mediaUrl?: string;
    price: number;
    categoryId: string;
    brandId: string;
    createdAt: string;
    specifications: Record<string, unknown>;
    availableQuantity: number;
}