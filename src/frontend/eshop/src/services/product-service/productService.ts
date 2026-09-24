import { apiService } from "../api-service/apiService.ts";
import type {ProductListItemResponse} from "../../models/products/responses/ProductListItemResponse.ts";
import type {ProductDetailsResponse} from "../../models/products/responses/ProductDetailsResponse.ts";

export const productService = {
    getAll: () => {
        return apiService.get<ProductListItemResponse[]>(
            "products");
    },
    getByCategoryId: (categoryId: string) => {
        return apiService.get<ProductListItemResponse[]>(
            `products/categories/${categoryId}`);
    },
    getById: (productId: string) =>
        apiService.get<ProductDetailsResponse>(
            `products/${productId}`
        ),
};