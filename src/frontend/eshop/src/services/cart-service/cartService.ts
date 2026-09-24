import { apiService } from "../api-service/apiService.ts";
import type {CartResponse} from "../../models/cart/responses/CartResponse.ts";

export const cartService = {
    get: () => {
        return apiService.get<CartResponse>(
            "cart");
    },
};