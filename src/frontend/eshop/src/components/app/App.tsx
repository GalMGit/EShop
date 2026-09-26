import { BrowserRouter as Router, Navigate, Route, Routes } from "react-router-dom";
import "./App.css";
import { Layout } from "../layout/Layout.tsx";
import { CategoryView } from "../categories/category-view/CategoryView.tsx";
import { Home } from "../home/Home.tsx";
import {ProductView} from "../products/product-view/ProductView.tsx";
import {LoginForm} from "../auth/login-rorm/LoginForm.tsx";
import {AuthLayout} from "../auth/auth-layout/AuthLayout.tsx";
import {RegisterForm} from "../auth/register-form/RegisterForm.tsx";
import {CartView} from "../cart/cart-view/CartView.tsx";
import {OrderMain} from "../orders/order-main/OrderMain.tsx";
import {OrderView} from "../orders/order-view/OrderView.tsx";

export const App = () => {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<Layout />}>
                    <Route index element={<Home />} />

                    <Route
                        path="categories/:categoryId"
                        element={<CategoryView />}
                    />
                    <Route
                        path="products/:productId"
                        element={<ProductView />}
                    />

                    <Route
                        path="cart"
                        element={<CartView />}
                    />

                    <Route path="orders"
                           element={<OrderMain />}
                    />
                    <Route path="orders/:orderId"
                           element={<OrderView />}
                    />
                </Route>

                <Route>
                    <Route path={"/auth"} element={<AuthLayout/>}>
                        <Route index element={<LoginForm/>}/>
                        <Route path={"login"} element={<LoginForm/>}/>
                        <Route path={"register"} element={<RegisterForm/>}/>
                    </Route>
                </Route>

                <Route
                    path="*"
                    element={<Navigate to="/" replace />}
                />
            </Routes>
        </Router>
    );
};