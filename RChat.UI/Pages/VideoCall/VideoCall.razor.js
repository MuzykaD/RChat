export function setLocalStream(stream) {
    const localVideo = document.getElementById('localVideo');
    localVideo.srcObject = stream;
}
export function setRemoteStream(stream) {
    const remoteVideo = document.getElementById('remoteVideo');
    remoteVideo.srcObject = stream;
}

export function setRemoteStreamToNull() {
    const remoteVideo = document.getElementById('remoteVideo');
    if (remoteVideo.srcObject) {
        // Get all tracks and stop them to release resources
        const tracks = remoteVideo.srcObject.getTracks();
        tracks.forEach(track => track.stop());

        // Set srcObject to null to clear the video
        remoteVideo.srcObject = null;
    }
}