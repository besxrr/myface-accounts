import React, {createContext, ReactNode, useState} from "react";
import encodeHeader from "../../Api/AuthHeader";
import {logIn as verifyLogInDetails} from "../../Api/apiClient";

interface ILoginContext {
    header?: { Authorization: string };
    userName?: string;
    isAdmin: boolean;
    logIn: (name: string, password: string) => void;
    logOut: () => void;
}

export const LoginContext = createContext<ILoginContext>({} as ILoginContext);

interface LoginManagerProps {
    children: ReactNode
}

export function LoginProvider(props: LoginManagerProps): JSX.Element {

    const [header, setHeader] = useState<undefined | { Authorization: string }>(undefined);
    const [userName, setUserName] = useState<string | undefined>(undefined);

    async function logIn(name: string, password: string) {
        const encodedHeader = encodeHeader(name, password)
        try {
            let userRole = await verifyLogInDetails(encodedHeader);
            // console.log(userRole)
            setHeader(encodedHeader)
            setUserName(name)
        } catch (e) {
            setHeader(undefined)
        }
    }

    function logOut() {
        setHeader(undefined);
    }

    const context = {
        isAdmin: header !== undefined,
        header: header,
        userName: userName,
        logIn: logIn,
        logOut: logOut,
    };

    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}