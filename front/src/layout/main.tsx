import { Outlet, Link } from "react-router";
import SiteHeader from "./header";

export default function Layout() {
    return (
        <>
            <SiteHeader />
            <Outlet />
        </>
    )
}
