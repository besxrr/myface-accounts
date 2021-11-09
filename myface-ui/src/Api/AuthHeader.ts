const encodeHeader = (username: string, password: string) => {
    const toEncode = username + ':' + password;
    return {"Authorization":`Basic ${btoa(toEncode)}`};
}
export default encodeHeader