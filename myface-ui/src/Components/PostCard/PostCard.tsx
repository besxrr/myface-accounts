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
    const [userHasLiked, setUserHasLiked] = useState(props.post.likes.some(i => i.user.username == loginContext.userName && i.type == InteractionType.LIKE));
    const [userHasDisliked, setUserHasDisliked] = useState(props.post.likes.some(i => i.user.username == loginContext.userName && i.type == InteractionType.DISLIKE));

    useEffect(() => {
        (async () => {
            const interactionCount = await fetchInteractionsForPost(props.post.id)
            setLikesCount(interactionCount.likes)
            setDislikesCount(interactionCount.dislikes)
        })()
    },[userHasLiked, userHasDisliked])

    const LikePost = async () => {
        await createInteraction({
            InteractionType: InteractionType.LIKE,
            PostId: props.post.id,
        }, loginContext.header);
        setUserHasLiked(!userHasLiked);
        setUserHasDisliked(!userHasDisliked);
    }

    const DislikePost = async () => {
        await createInteraction({
            InteractionType: InteractionType.DISLIKE,
            PostId: props.post.id,
        }, loginContext.header);
        setUserHasLiked(!userHasLiked);
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
                    <p>👍 {likesCount}</p>
                    <button onClick={LikePost}>Like</button>
                    <p>👎 {dislikesCount}</p>
                    <button onClick={DislikePost} >Dislike</button>
                    {/*Todo make so user can only like/dislike something once*/}
                    {/*Todo visual indication that user has liked/disliked*/}
                </div>
            </div>
        </Card>
    );
}