import { useEffect, useState } from "react";
import "./CartView.css";
import type {CartResponse} from "../../../models/cart/responses/CartResponse.ts";
import {cartService} from "../../../services/cart-service/cartService.ts";

export const CartView = () => {
    const [cart, setCart] = useState<CartResponse | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const loadCart = async () => {
            try {
                setLoading(true);
                setError(null);

                const response = await cartService.get();

                setCart(response.data);
            } catch (error) {
                console.error(error);
                setError("Failed to load cart.");
            } finally {
                setLoading(false);
            }
        };

        loadCart();
    }, []);

    if (loading) {
        return (
            <main className="cart-view">
                <div className="cart-status">
                    Loading cart...
                </div>
            </main>
        );
    }

    if (error) {
        return (
            <main className="cart-view">
                <div className="cart-status cart-error">
                    {error}
                </div>
            </main>
        );
    }

    if (!cart || cart.items.length === 0) {
        return (
            <main className="cart-view">
                <div className="cart-header">
                    <span className="cart-label">SHOPPING CART</span>
                    <h1>Your cart</h1>
                </div>

                <div className="cart-empty">
                    <h2>Your cart is empty</h2>
                    <p>
                        Add some products to your cart to see them here.
                    </p>
                </div>
            </main>
        );
    }

    return (
        <main className="cart-view">
            <div className="cart-header">
                <span className="cart-label">SHOPPING CART</span>

                <h1>Your cart</h1>

                <p>
                    {cart.items.length}{" "}
                    {cart.items.length === 1 ? "item" : "items"}
                </p>
            </div>

            <div className="cart-content">
                <section className="cart-items">
                    {cart.items.map((item) => (
                        <article
                            className="cart-item"
                            key={item.id}
                        >
                            <div className="cart-item-image">
                                {item.thumbnailUrl ? (
                                    <img
                                        src={item.thumbnailUrl}
                                        alt={item.productName}
                                    />
                                ) : (
                                    <div className="cart-item-placeholder">
                                        No image
                                    </div>
                                )}
                            </div>

                            <div className="cart-item-info">
                                <h2>{item.productName}</h2>

                                <span className="cart-item-price">
                                    {item.unitPrice.toFixed(2)}Р
                                </span>
                            </div>

                            <div className="cart-item-quantity">
                                <span>Qty</span>
                                <strong>{item.quantity}</strong>
                            </div>

                            <div className="cart-item-total">
                                {item.totalPrice.toFixed(2)}Р
                            </div>
                        </article>
                    ))}
                </section>

                <aside className="cart-summary">
                    <span className="cart-summary-label">
                        ORDER SUMMARY
                    </span>

                    <div className="cart-summary-row">
                        <span>Subtotal</span>

                        <strong>
                            {cart.total.toFixed(2)}Р
                        </strong>
                    </div>

                    <div className="cart-summary-divider" />

                    <div className="cart-summary-total">
                        <span>Total</span>

                        <strong>
                            {cart.total.toFixed(2)}Р
                        </strong>
                    </div>

                    <button
                        className="cart-checkout-button"
                        type="button"
                    >
                        Checkout
                    </button>
                </aside>
            </div>
        </main>
    );
};