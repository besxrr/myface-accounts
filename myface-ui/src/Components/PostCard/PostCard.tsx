import React, {useContext, useEffect, useState} from "react";
import {createInteraction, fetchInteractionsForPost, InteractionType, Post} from "../../Api/apiClient";
import {Card} from "../Card/Card";
import "./PostCard.scss";
import {Link} from "react-router-dom";
import {LoginContext} from "../LoginProvider/LoginProvider";

interface PostCardProps {
    post: Post;
}

export function PostCard(props: PostCardProps): JSX.Element {
    const loginContext = useContext(LoginContext);
    const [likesCount, setLikesCount] = useState(props.post.likes.length);
    const [dislikesCount, setDislikesCount] = useState(props.post.dislikes.length);
    const [userHasLiked, setUserHasLiked] = useState(props.post.likes.some(i => (i.user.username == loginContext.userName && i.type.toString() == "LIKE")));
    const [userHasDisliked, setUserHasDisliked] = useState(props.post.dislikes.some(i => (i.user.username == loginContext.userName && i.type.toString() == "DISLIKE")));
    //TODO remove username from context

    useEffect(() => {
        (async () => {
            const interactionCount = await fetchInteractionsForPost(props.post.id)
            setLikesCount(interactionCount.likes)
            setDislikesCount(interactionCount.dislikes)
            console.log(props.post.likes.some(i => (i.user.username == loginContext.userName && i.type == InteractionType.LIKE)))
            console.log(props.post.likes.map(i => i.type))
        })()
    },[userHasLiked, userHasDisliked])

    const LikePost = async () => {
        await createInteraction({
            InteractionType: InteractionType.LIKE,
            PostId: props.post.id,
        }, loginContext.header);
        setUserHasLiked(!userHasLiked);
        setUserHasDisliked(false);
    }

    const DislikePost = async () => {
        await createInteraction({
            InteractionType: InteractionType.DISLIKE,
            PostId: props.post.id,
        }, loginContext.header);
        setUserHasLiked(false);
        setUserHasDisliked(!userHasDisliked);
    }

    return (
        <Card>
            <div className="post-card">
                <img className="image" src={props.post.imageUrl} alt=""/>
                <div className="message">{props.post.message}</div>
                <div className="user">
                    <img className="profile-image" src={props.post.postedBy.profileImageUrl} alt=""/>
                    <Link className="user-name" to={`/users/${props.post.postedBy.id}`}>{props.post.postedBy.displayName}</Link>
                </div>
                <div className="interactions">
                    <p className={userHasLiked ? "selected" : ""}>👍 {likesCount}</p>
                    <button onClick={LikePost}>Like</button>
                    <p className={userHasDisliked ? "selected" : ""}>👎 {dislikesCount}</p>
                    <button onClick={DislikePost} >Dislike</button>
                </div>
            </div>
        </Card>
    );
}