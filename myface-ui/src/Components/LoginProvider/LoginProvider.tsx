﻿import React, {createContext, ReactNode, useState} from "react";
import encodeHeader from "../../Api/AuthHeader";
import {logIn as verifyLogInDetails}  from "../../Api/apiClient";

interface ILoginContext{
    header?: {Authorization:string};
    isAdmin: boolean;
    logIn:(name:string, password:string) => void;
    logOut: () => void;
}

export const LoginContext = createContext<ILoginContext>({} as ILoginContext);

interface LoginManagerProps {
    children: ReactNode
}

export function LoginProvider(props: LoginManagerProps): JSX.Element {

    const [header, setHeader] = useState<undefined|{Authorization:string}>(undefined);

    async function  logIn(name:string, password:string) {
        const encodedHeader = encodeHeader(name, password)
        try{
            await verifyLogInDetails(encodedHeader);
            //If successful, update context to store the value of the HEADER
            //Store the user info
            setHeader(encodedHeader)
        }
        catch (e){
            setHeader(undefined)
        }
    }
    
    function logOut() {
        setHeader(undefined);
    }
    
    const context = {
        isAdmin: header !== undefined,
        header: header,
        logIn: logIn,
        logOut: logOut,
    };
    
    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}