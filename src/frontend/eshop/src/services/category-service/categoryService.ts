import { apiService } from "../api-service/apiService.ts";
import type {CategoryResponse} from "../../models/categories/responses/CategoryResponse.ts";

export const categoryService = {
    getAll: () => {
        return apiService.get<CategoryResponse[]>("categories");
    },
};