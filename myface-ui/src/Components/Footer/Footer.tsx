import React, {useContext} from "react";
import "./Footer.scss";
import {LoginContext} from "../LoginProvider/LoginProvider";

export function Footer(): JSX.Element {
    const loginContext = useContext(LoginContext);

    function logout() {
        loginContext.logOut();
    }
    
    return (
        <footer className="footer">
            <p>Made for <a className="link" href="https://techswitch.co.uk">TechSwitch</a>.</p>
            <button className="log-out-button" onClick={logout}>Log Out</button>
        </footer>
    );
}