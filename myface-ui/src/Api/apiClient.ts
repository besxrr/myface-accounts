﻿import {LoginContext} from "../Components/LoginProvider/LoginProvider";
import {create} from "domain";

export interface ListResponse<T> {
    items: T[];
    totalNumberOfItems: number;
    page: number;
    nextPage: string;
    previousPage: string;
}

export interface User {
    id: number;
    firstName: string;
    lastName: string;
    displayName: string;
    username: string;
    email: string;
    profileImageUrl: string;
    coverImageUrl: string;
}

export interface Interaction {
    id: number;
    user: User;
    type: string;
    date: string;
}

export interface Post {
    id: number;
    message: string;
    imageUrl: string;
    postedAt: string;
    postedBy: User;
    likes: Interaction[];
    dislikes: Interaction[];
}

export interface NewPost {
    message: string;
    imageUrl: string;
}

export interface AuthHeader {
    Authorization: string;
}

export async function fetchUsers(searchTerm: string, page: number, pageSize: number, authHeader: AuthHeader | undefined): Promise<ListResponse<User>> {
    const response = await fetch(`https://localhost:5001/users?search=${searchTerm}&page=${page}&pageSize=${pageSize}`, {
        method: "GET",
        headers: {
            ...authHeader
        }
    });
    if(!response.ok){
        throw new Error(await response.json())
    }
    return await response.json();
}

export async function fetchUser(userId: string | number, authHeader: AuthHeader | undefined): Promise<User> {
    const response = await fetch(`https://localhost:5001/users/${userId}`, {
        method: "GET",
        headers: {
            ...authHeader
        }
    });
    if(!response.ok){
        throw new Error(await response.json())
    }
    return await response.json();
}

export async function fetchPosts(page: number, pageSize: number, authHeader: AuthHeader | undefined): Promise<ListResponse<Post>> {
    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}`, {
        method: "GET",
        headers: {
            ...authHeader
        }
    });
    if(!response.ok){
        throw new Error(await response.json())
    }
    return await response.json();
}

export async function fetchPostsForUser(page: number, pageSize: number, userId: string | number, authHeader: AuthHeader | undefined) {
    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&postedBy=${userId}`, {
        method: "GET",
        headers: {
            ...authHeader
        }
    });
    if(!response.ok){
        throw new Error(await response.json())
    }
    return await response.json();
}

export async function fetchPostsLikedBy(page: number, pageSize: number, userId: string | number) {
    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&likedBy=${userId}`);
    return await response.json();
}

export async function fetchPostsDislikedBy(page: number, pageSize: number, userId: string | number) {
    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&dislikedBy=${userId}`);
    return await response.json();
}

export async function createPost(newPost: NewPost, authHeader: AuthHeader | undefined) {
    const response = await fetch(`https://localhost:5001/posts/create`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            ...authHeader
        },
        body: JSON.stringify(newPost),
    });
    
    if (!response.ok) {
        throw new Error(await response.json())
    }
}

export async function logIn(authHeader:{Authorization:string}){
    const response = await fetch(`https://localhost:5001/login`,{
        method: "GET",
        headers: {
            ...authHeader
        }
    })
    if(!response.ok){
        throw new Error("Login Failed!")
    }
    return true;
}

