﻿import React, {useContext, useEffect, useState} from 'react';
import {fetchUser, RoleType, User} from "../../Api/apiClient";
import "./UserDetails.scss";
import {LoginContext} from "../LoginProvider/LoginProvider";
import {deleteUser} from "../../Api/apiClient";

interface UserDetailsProps {
    userId: string;
}

export function UserDetails(props: UserDetailsProps): JSX.Element {
    const [user, setUser] = useState<User | null>(null);
    const [hasDeleted, setHasDeleted] = useState(false);
    const loginContext = useContext(LoginContext)
    
    useEffect(() => {
        fetchUser(props.userId, loginContext.header)
            .then(response => setUser(response));
    }, [props, hasDeleted]);

    const handleDelete = () => {
        deleteUser(parseInt(props.userId),loginContext.header)
        setHasDeleted(true);
        setUser(null);
    }
    
    if (!user) {
        return <section>Loading...</section>
    }
    
    return (
        <section className="user-details">
            <img className="cover-image" src={user.coverImageUrl} alt="A cover image for the user."/>
            <div className="user-info">
                <img className="profile-image" src={user.profileImageUrl} alt=""/>
                <div className="contact-info">
                    <h1 className="title">{user.displayName}</h1>
                    <div className="username">{user.username}</div>
                    <div className="email">{user.email}</div>
                </div>
            </div>
            <button className="delete-user" style={{display: loginContext.userRole === RoleType.ADMIN ? 'block' : 'none'}} onClick={handleDelete}>Delete User</button>
        </section>
    );
}
