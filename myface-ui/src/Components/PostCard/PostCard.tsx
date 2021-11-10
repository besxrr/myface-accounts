import React, {useContext} from "react";
import {createInteraction, InteractionType, Post} from "../../Api/apiClient";
import {Card} from "../Card/Card";
import "./PostCard.scss";
import {Link} from "react-router-dom";
import {LoginContext} from "../LoginProvider/LoginProvider";

interface PostCardProps {
    post: Post;
}

export function PostCard(props: PostCardProps): JSX.Element {
    const loginContext = useContext(LoginContext);

    const LikePost = async () => {
        createInteraction({
            InteractionType: InteractionType.LIKE,
            PostId: props.post.id,
        }, loginContext.header)
    }

    const DislikePost = async () => {
        createInteraction(  {
            InteractionType: InteractionType.DISLIKE,
            PostId: props.post.id,
        }, loginContext.header)
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
                    <p>👍 {props.post.likes.length}</p>
                    <button onClick={LikePost}>Like</button>
                    <p>👎 {props.post.dislikes.length}</p>
                    <button onClick={DislikePost} >Dislike</button>
                    {/*Todo make so user can only like/dislike something once*/}
                    {/*Todo visual indication that user has liked/disliked*/}
                </div>
            </div>
        </Card>
    );
}