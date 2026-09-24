import {Outlet} from "react-router-dom";

export const AuthLayout = () => {
    return (
        <div className={"auth-container"}>
            <div className={"auth-place"}>
                <Outlet/>
            </div>
        </div>
    )
}