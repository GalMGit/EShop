import { apiService } from "../api-service/apiService.ts";
import type {ProductListItemResponse} from "../../models/products/responses/ProductListItemResponse.ts";

export const productService = {
    getAll: () => {
        return apiService.get<ProductListItemResponse[]>("products");
    },
};