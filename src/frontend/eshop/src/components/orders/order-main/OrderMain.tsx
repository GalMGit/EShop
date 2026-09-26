import "./OrderMain.css";
import {OrderList} from "../order-list/OrderList.tsx";

export const OrderMain = () => {
    return (
        <main className="orders-view">
            <div className="orders-header">
                <span className="orders-label">
                    ORDERS
                </span>

                <h1>Your orders</h1>

                <p>
                    View your order history and current order status.
                </p>
            </div>

            <OrderList />
        </main>
    );
};